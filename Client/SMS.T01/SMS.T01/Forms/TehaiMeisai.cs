//#define __DEBUG_ENABLE_CODE__     // デバッグ用コードのため、定義状態でアップしないこと
using System.Reflection;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using WsConnection.WebRefT01;
using WsConnection.WebRefSms;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using ElTabelleHelper;
using SMS.T01.Properties;
using GrapeCity.Win.ElTabelle;
using GrapeCity.Win.ElTabelle.Editors;
using XlsCreatorHelper;
using XlsxCreatorHelper;



namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細登録
    /// </summary>
    /// <create>S.Furugo 2018/10/23</create>
    /// <update></update>
    /// <remarks>
    /// <p>
    /// ＜注意事項＞
    /// ・入力チェックはSheet側のデータで処理すること※DataSauce側で行わない(行数や並びが表示(Sheet)と異なるため)
    /// ・シートのフィールド名をデザイナーで設定しており、変更の際には注意すること
    /// </p>
    /// </remarks>
    /// --------------------------------------------------
    public partial class TehaiMeisai : SystemBase.Forms.CustomOrderForm
    {
        #region Enum定義
        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>S.Furugo 2018/10/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            Initialize,     // 初期
            Insert,         // 新規
            Update,         // 更新
            Delete          // 全削除
        }

        /// --------------------------------------------------
        /// <summary>
        /// 列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/09</create>
        /// <update>H.Tsuji 2019/08/24 STEP10 組立パーツの識別処理を追加</update>
        /// <update>T.Nukaga 2019/11/15 STEP12 返却品管理対応 HENKYAKUHIN_FLAG追加</update>
        /// <update>J.Chen 2022/04/12 STEP14 列追加</update>
        /// <update>H.Iimuro 2022/10/19 STEP15 列追加</update>
        /// <update>Y.Gwon 2023/07/04 STEP16 返却品を出荷制限に変更</update>
        /// <update>J.Chen 2024/10/23 変更履歴追加</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <remarks>
        /// <p>
        /// RowFillingイベントやRowUpdatingイベントなどで列インデックスを使用して値を取得する箇所がある。
        /// 列インデックス、シートの列順、表示用に取得した手配明細データは必ず順番を統一する事。
        /// </p>
        /// </remarks>
        /// --------------------------------------------------
        private enum SHEET_COL
        {
            TEHAI_CHECK = 0,        // チェックボックス
            TEHAI_RENKEI_NO,        // 連携No.
            ECS_QUOTA,		        // ECS期			非表示
            COL_ECS_NO,		        // ECS No.			非表示
            SETTEI_DATE,		    // 設定納期
            NOUHIN_SAKI,		    // 納品先
            SYUKKA_SAKI,		    // 出荷先
            FLOOR,		            // Floor
            ST_NO,		            // ST-No.
            ZUMEN_OIBAN,		    // 図面追番
            HINMEI_JP,		        // 品名(和文)
            HINMEI,		            // 品名
            ZUMEN_KEISHIKI,		    // 図番/型式
            ZUMEN_KEISHIKI_SEARCH,  // 図番/型式検索	非表示
            ZUMEN_KEISHIKI2,		// 図番/型式2
            HINMEI_INV,		        // INV付加名
            TEHAI_QTY,		        // 手配数
            QUANTITY_UNIT,		    // 数量単位
            TEHAI_FLAG,		        // 手配区分
            TEHAI_NO,		        // 手配No.
            TEHAI_KIND_FLAG,		// 手配種別			非表示
            HENKYAKUHIN_FLAG,       // 返却品→出荷制限
            HACCHU_QTY,		        // 発注数
            SHUKKA_QTY,		        // 出荷数
            ASSY_NO,                // Assy no.
            FREE1,		            // Free1
            FREE2,		            // Free2
            NOTE,		            // 備考
            NOTE2,                  // 備考2
            NOTE3,                  // 備考3
            CUSTOMS_STATUS,         // 通関確認状態
            MAKER,		            // Maker
            UNIT_PRICE,		        // 単価(JPY)
            PHOTO,                  // 写真
            TEHAI_RIREKI,           // 変更履歴
            CREATE_USER_NAME,       // 登録ユーザー名
            CREATE_DATE,            // 登録日時
            ARRIVAL_QTY,		    // 入荷数			非表示
            ASSY_QTY,		        // 組立数			非表示
            ESTIMATE_FLAG,		    // 有償				非表示
            ESTIMATE_NO,		    // 見積No.			非表示
            VERSION,		        // バージョン		非表示
            SHUKKA_MEISAI_CNT,		// TAG連携状態      非表示
            SHUKKA_MEISAI_QTY,	    // 出荷明細数量(合計)	非表示
            ESTIMATE_COLOR,         // 見積状態色       非表示
            SHUKKAZUMI_CNT,         // 出荷済み明細件数       非表示
        }

        /// --------------------------------------------------
        /// <summary>
        /// 見積モード
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected enum EstimateMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 未指定
            /// </summary>
            /// <create>H.Iimuro 2022/10/19</create>
            /// <update></update>
            /// --------------------------------------------------
            Neutral,
            /// --------------------------------------------------
            /// <summary>
            /// 無償
            /// </summary>
            /// <create>H.Iimuro 2022/10/19</create>
            /// <update></update>
            /// --------------------------------------------------
            Gratis,
            /// --------------------------------------------------
            /// <summary>
            /// 有償
            /// </summary>
            /// <create>H.Iimuro 2022/10/19</create>
            /// <update></update>
            /// --------------------------------------------------
            Onerous,
        }
        #endregion

        #region 定義
        /// --------------------------------------------------
        /// <summary>
        /// 初期表示時列インデックス
        /// </summary>
        /// <create>D.Naito 2018/12/07</create>
        /// <update>H.Iimuro 2022/10/19</update>
        /// --------------------------------------------------
        //private const int SHEET_TOPLEFT_COL = (int)SHEET_COL.TEHAI_RENKEI_NO;
        private const int SHEET_TOPLEFT_COL = (int)SHEET_COL.TEHAI_CHECK;
        /// --------------------------------------------------
        /// <summary>
        /// グリッドの入力チェッククラス
        /// </summary>
        /// <create>D.Naito 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private class CheckColumn
        {
            public int col { get; set; }        // 列
            public string label { get; set; }   // ヘッダーラベル名
            public int pattern { get; set; }    // チェック内容

            public CheckColumn(int col, string label, int pattern)
            {
                this.col = col;
                this.label = label;
                this.pattern = pattern;
            }
        };

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの入力チェックリスト
        /// </summary>
        /// <create>D.Naito 2018/11/26</create>
        /// <update>D.Okumura 2019/01/22 図番/型式を必須入力チェックから除外</update>
        /// <update>H.Tsuji 2019/08/26 STEP10 組立パーツの識別処理を追加</update>
        /// <update>T.Nukaga 2019/11/18 STEP12 返却品管理対応</update>
        /// <update>J.Chen 2023/02/21 STEP15 品名半角英数字チェック</update>
        /// --------------------------------------------------
        private List<CheckColumn> _inputCheckColumns = new List<CheckColumn>(){
              new CheckColumn((int)SHEET_COL.SETTEI_DATE, Resources.TehaiMeisai_SettingDate, 1)
            , new CheckColumn((int)SHEET_COL.HINMEI_JP, Resources.TehaiMeisai_JpName, 1)
            , new CheckColumn((int)SHEET_COL.HINMEI, Resources.TehaiMeisai_Name, 6)
            //, new CheckColumn((int)SHEET_COL.ZUMEN_KEISHIKI, Resources.TehaiMeisai_PartNo, 1)
            , new CheckColumn((int)SHEET_COL.TEHAI_QTY, Resources.TehaiMeisai_ArrangementQty, 2)
            , new CheckColumn((int)SHEET_COL.TEHAI_FLAG, Resources.TehaiMeisai_ArrangementFlag, 1) // 注意コンボボックス(未選択Value値)
            , new CheckColumn((int)SHEET_COL.HENKYAKUHIN_FLAG, Resources.TehaiMeisai_HenkyakuhinFlag, 1) // 注意コンボボックス(未選択Value値)
            , new CheckColumn((int)SHEET_COL.ASSY_NO, Resources.TehaiMeisai_AssyNo, 1)
            , new CheckColumn((int)SHEET_COL.HACCHU_QTY, Resources.TehaiMeisai_OrderQty, 3)
            , new CheckColumn((int)SHEET_COL.SHUKKA_QTY, Resources.TehaiMeisai_ShippingQty, 4)
            , new CheckColumn((int)SHEET_COL.FREE2, Resources.TehaiMeisai_Free2, 5)
            , new CheckColumn((int)SHEET_COL.QUANTITY_UNIT, Resources.TehaiMeisai_QtyUnit, 1)     // 注意コンボボックス(未選択Value値)
            , new CheckColumn((int)SHEET_COL.MAKER, Resources.TehaiMeisai_Maker, 1)
        };

        /// --------------------------------------------------
        /// <summary>
        /// 一括CopyのCopy列リスト
        /// </summary>
        /// <create>D.Naito 2018/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private int[] _copyColums = new int[]
        {
              (int)SHEET_COL.SETTEI_DATE
            , (int)SHEET_COL.NOUHIN_SAKI
            , (int)SHEET_COL.SYUKKA_SAKI
            , (int)SHEET_COL.FLOOR
            , (int)SHEET_COL.ST_NO
        };

        /// --------------------------------------------------
        /// <summary>
        /// 分割のCopy列リスト
        /// </summary>
        /// <create>D.Naito 2018/12/03</create>
        /// <update>H.Tsuji 2019/08/26 STEP10 組立パーツの識別処理を追加</update>
        /// <update>T.Nukaga 2019/11/18 STEP12 返却品管理対応</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// --------------------------------------------------
        private int[] _copyBunkatuColums = new int[]
        {
              (int)SHEET_COL.SETTEI_DATE
            , (int)SHEET_COL.NOUHIN_SAKI
            , (int)SHEET_COL.SYUKKA_SAKI
            , (int)SHEET_COL.ZUMEN_OIBAN
            , (int)SHEET_COL.FLOOR
            , (int)SHEET_COL.ST_NO
            , (int)SHEET_COL.HINMEI_JP
            , (int)SHEET_COL.HINMEI
            , (int)SHEET_COL.HINMEI_INV
            , (int)SHEET_COL.ZUMEN_KEISHIKI
            //, (int)SHEET_COL.TEHAI_QTY
            , (int)SHEET_COL.TEHAI_FLAG
            , (int)SHEET_COL.HENKYAKUHIN_FLAG
            //, (int)SHEET_COL.HACCHU_QTY
            //, (int)SHEET_COL.SHUKKA_QTY
            , (int)SHEET_COL.FREE1
            , (int)SHEET_COL.FREE2
            , (int)SHEET_COL.QUANTITY_UNIT
            , (int)SHEET_COL.ZUMEN_KEISHIKI2
            , (int)SHEET_COL.NOTE
            , (int)SHEET_COL.NOTE2
            , (int)SHEET_COL.NOTE3
            , (int)SHEET_COL.CUSTOMS_STATUS
            , (int)SHEET_COL.MAKER
            , (int)SHEET_COL.UNIT_PRICE
            //, (int)SHEET_COL.ARRIVAL_QTY
            //, (int)SHEET_COL.ASSY_QTY
            , (int)SHEET_COL.ESTIMATE_FLAG
            , (int)SHEET_COL.ESTIMATE_NO
        };

        /// --------------------------------------------------
        /// <summary>
        /// 行Copy列リスト
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/08</create>
        /// <update>H.Tsuji 2019/08/26 STEP10 組立パーツの識別処理を追加</update>
        /// <update>T.Nukaga 2019/11/18 STEP12 返却品管理対応</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// --------------------------------------------------
        private int[] _rowCopyColums = new int[]
        {
              (int)SHEET_COL.HINMEI
            , (int)SHEET_COL.SETTEI_DATE
            , (int)SHEET_COL.NOUHIN_SAKI
            , (int)SHEET_COL.SYUKKA_SAKI
            //, (int)SHEET_COL.ZUMEN_OIBAN
            , (int)SHEET_COL.FLOOR
            , (int)SHEET_COL.ST_NO
            , (int)SHEET_COL.HINMEI_JP
            , (int)SHEET_COL.HINMEI_INV
            , (int)SHEET_COL.ZUMEN_KEISHIKI
            //, (int)SHEET_COL.TEHAI_QTY
            , (int)SHEET_COL.TEHAI_FLAG
            , (int)SHEET_COL.HENKYAKUHIN_FLAG
            //, (int)SHEET_COL.HACCHU_QTY
            //, (int)SHEET_COL.SHUKKA_QTY
            , (int)SHEET_COL.FREE1
            , (int)SHEET_COL.FREE2
            , (int)SHEET_COL.QUANTITY_UNIT
            , (int)SHEET_COL.ZUMEN_KEISHIKI2
            , (int)SHEET_COL.NOTE
            , (int)SHEET_COL.NOTE2
            , (int)SHEET_COL.NOTE3
            , (int)SHEET_COL.CUSTOMS_STATUS
            , (int)SHEET_COL.MAKER
            //, (int)SHEET_COL.UNIT_PRICE
            //, (int)SHEET_COL.ARRIVAL_QTY
            //, (int)SHEET_COL.ASSY_QTY
            //, (int)SHEET_COL.ESTIMATE_FLAG
            //, (int)SHEET_COL.ESTIMATE_NO
        };

        /// --------------------------------------------------
        /// <summary>
        /// 編集・削除時の技連マスタバージョン情報
        /// </summary>
        /// <create>D.Naito 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DateTime _girenVerOrg { get; set; }

        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスの列インデックス
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CHECK = 0;

        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスONの値
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECK_ON = 1;
        private const string CHECK_ON_STR = "1";

        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスOFFの値
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECK_OFF = 0;
        private const string CHECK_OFF_STR = "0";
        /// --------------------------------------------------
        /// <summary>
        /// 見積状態色
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update>J.Chen 2024/10/23 変更履歴の追加により+1</update>
        /// <update>J.Chen 2024/11/07 通関確認状態の追加により+1</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int ESTIMATE_COLOR = 44;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 手配連携No.の列インデックス
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TEHAI_RENKEI_NO = 1;
        /// --------------------------------------------------
        /// <summary>
        /// バージョンの列インデックス
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update>J.Chen 2024/10/23 変更履歴の追加により+1</update>
        /// <update>J.Chen 2024/11/07 通関確認状態の追加により+1</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_VERSION = 41;
        /// --------------------------------------------------
        /// <summary>
        /// 見積状態
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update>J.Chen 2024/10/23 変更履歴の追加により+1</update>
        /// <update>J.Chen 2024/11/07 通関確認状態の追加により+1</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ESTIMATE_FLAG = 39;
        /// --------------------------------------------------
        /// <summary>
        /// 見積ID
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int ESTIMATE_FLAG_GRATIS = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 見積ID
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int ESTIMATE_FLAG_ONEROUS = 1;
        /// --------------------------------------------------
        /// <summary>
        /// 見積ID
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int ESTIMATE_FLAG_NEUTRAL = 9;
        /// --------------------------------------------------
        /// <summary>
        /// 見積No.の列インデックス
        /// </summary>
        /// <create>J.Chen 2022/11/14</create>
        /// <update>J.Chen 2024/10/23 変更履歴の追加により+1</update>
        /// <update>J.Chen 2024/11/07 通関確認状態の追加により+1</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ESTIMATE_NO = 40;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷済み明細件数の列インデックス
        /// </summary>
        /// <create>J.Chen 2022/11/14</create>
        /// <update>J.Chen 2024/10/23 変更履歴の追加により+1</update>
        /// <update>J.Chen 2024/11/07 通関確認状態の追加により+1</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CNT = 45;

        #endregion

        #region フィールド定義

        /// --------------------------------------------------
        /// <summary>
        /// 期 - 外部連携用
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _ecsQuata = 0;
        /// --------------------------------------------------
        /// <summary>
        /// ECS No. - 外部連携用
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ecsNo = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 外部連携モード
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _dialogMode = false;
        /// --------------------------------------------------
        /// <summary>
        /// Iniファイルのセクション名プレフィックス
        /// </summary>
        /// <create>H.Tsuji 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly string _iniSectionPrefix = string.Empty;

        // ===================================================
        // 行入れ替え用
        // ===================================================

        /// --------------------------------------------------
        /// <summary>
        /// 行入れ替え - 座標
        /// </summary>
        /// <create>J.Chen 2022/05/18 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        private Rectangle dragBox;
        /// --------------------------------------------------
        /// <summary>
        /// 行入れ替え - 移動元行番号
        /// </summary>
        /// <create>J.Chen 2022/05/18 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        private int sourceRow;
        /// --------------------------------------------------
        /// <summary>
        /// 行入れ替え - 移動元行番号(削除処理後)
        /// </summary>
        /// <create>J.Chen 2022/05/18 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        private int sourceRowTemp;
        /// --------------------------------------------------
        /// <summary>
        /// 行入れ替え - 移動先行番号
        /// </summary>
        /// <create>J.Chen 2022/05/18 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        private int destinationRow;
        /// --------------------------------------------------
        /// <summary>
        /// 行入れ替え - 行入れ替えフラグ
        /// </summary>
        /// <create>J.Chen 2022/05/18 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool isRowChange = false;
        /// --------------------------------------------------
        /// <summary>
        /// 見積状態
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private EstimateMode EstimateStatus { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 手配No区切文字
        /// </summary>
        /// <create>J.Chen 2022/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private string delimiter = "+";
        /// --------------------------------------------------
        /// <summary>
        /// 編集前のデータテーブル
        /// </summary>
        /// <create>J.Chen 2022/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable dtTemp = null;
        /// --------------------------------------------------
        /// <summary>
        /// 返却品フラグ変更
        /// </summary>
        /// <create>J.Chen 2023/10/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool isHenkyakuhinFlagChange = false;
        /// --------------------------------------------------
        /// <summary>
        /// 明細分割
        /// </summary>
        /// <create>J.Chen 2023/10/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool isMeisaiBunkatsu = false;
        /// --------------------------------------------------
        /// <summary>
        /// 一時保存するためのテーブル
        /// </summary>
        /// <create>J.Chen 2023/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable tempTable = null;
        /// --------------------------------------------------
        /// <summary>
        /// 見積No取得
        /// </summary>
        /// <create>J.Chen 2023/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private String[] estimateNoList = null;
        /// --------------------------------------------------
        /// <summary>
        /// 更新比較用dt
        /// </summary>
        /// <create>J.Chen 2024/11/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _updCheckDt = null;

        #endregion

        #region コンストラクタ
        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>S.Furugo 2018/10/23</create>
        /// <update>K.Tsutsumi 2019/03/09 編集、Ship照会追加</update>
        /// --------------------------------------------------
        public TehaiMeisai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
            this._ecsQuata = 0;
            this._ecsNo = string.Empty;
            this._dialogMode = false;
            this._iniSectionPrefix = this.GetType().FullName;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ - 外部連携用
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="title">画面タイトル</param>
        /// <param name="ecsQuota"></param>
        /// <param name="ecsNo"></param>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiMeisai(UserInfo userInfo, string title, int ecsQuota, string ecsNo)
            : base(userInfo, title)
        {
            InitializeComponent();
            this._ecsQuata = ecsQuota;
            this._ecsNo = ecsNo;
            this._dialogMode = true;
            this._iniSectionPrefix = this.GetType().FullName;
        }

        #endregion

        #region 初期化
        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>S.Furugo 2018/10/23</create>
        /// <update>H.Tsuji 2019/08/01 Enterキー入力時のアクティブセル移動方向を制御</update>
        /// <update>D.Okumura 2019/12/12 期が2桁以下の場合、手配明細から検索失敗する問題を修正</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                this.DisplayClear();
                this.InitializeSheet(this.shtMeisai);   // シートの初期化
                this.ChangeMode(DisplayMode.Initialize);

                if (this._dialogMode)
                {
                    // 編集モードにする。
                    this.rdoChange.Checked = true;
                    // 検索条件設定
                    this.txtEcsQuota.Text = this._ecsQuata.ToString("000");
                    this.txtEcsNo.Text = this._ecsNo;
                    this.btnStart_Click(this, new EventArgs());
                }

                this.rdoInsert.Focus();

                // Enterキー操作ラジオボタン初期化
                // ラジオボタンはチェックするとTabStopも自動でtrueとなるため、その都度falseにする必要がある
                this.rdoRight.Checked = true;
                this.rdoRight.TabStop = false;
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
        /// <create>S.Furugo 2018/10/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Sheet初期化

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化を行うメソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <create>S.Furugo 2018/11/09</create>
        /// <update>H.Tsuji 2019/07/29 グリッド幅のユーザー設定</update>
        /// <update>H.Tsuji 2019/08/24 STEP10 組立パーツの識別処理を追加</update>
        /// <update>T.Nukaga 2019/11/18 STEP12 返却品管理対応 返却品タイトル追加</update>
        /// <update>J.Chen 2022/04/12 STEP14 列追加</update>
        /// <update>H.Iimuro 2022/10/19 STEP15 列追加</update>
        /// <update>J.Chen 2024/10/23 履歴更新列追加</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            try
            {
                base.InitializeSheet(sheet);

                // シートのタイトルを設定
                sheet.ColumnHeaders[(int)SHEET_COL.TEHAI_CHECK].Caption = "";
                sheet.ColumnHeaders[(int)SHEET_COL.TEHAI_RENKEI_NO].Caption = Resources.TehaiMeisai_CooperationNo;
                sheet.ColumnHeaders[(int)SHEET_COL.SETTEI_DATE].Caption = Resources.TehaiMeisai_SettingDate;
                sheet.ColumnHeaders[(int)SHEET_COL.NOUHIN_SAKI].Caption = Resources.TehaiMeisai_DeliveryDestination;
                sheet.ColumnHeaders[(int)SHEET_COL.SYUKKA_SAKI].Caption = Resources.TehaiMeisai_ShippingDestination;
                sheet.ColumnHeaders[(int)SHEET_COL.FLOOR].Caption = Resources.TehaiMeisai_Floor;
                sheet.ColumnHeaders[(int)SHEET_COL.ST_NO].Caption = Resources.TehaiMeisai_STNo;
                sheet.ColumnHeaders[(int)SHEET_COL.ZUMEN_OIBAN].Caption = Resources.TehaiMeisai_DwgNo;
                sheet.ColumnHeaders[(int)SHEET_COL.HINMEI_JP].Caption = Resources.TehaiMeisai_JpName;
                sheet.ColumnHeaders[(int)SHEET_COL.HINMEI].Caption = Resources.TehaiMeisai_Name;
                sheet.ColumnHeaders[(int)SHEET_COL.ZUMEN_KEISHIKI].Caption = Resources.TehaiMeisai_PartNo;
                sheet.ColumnHeaders[(int)SHEET_COL.ZUMEN_KEISHIKI2].Caption = Resources.TehaiMeisai_PartNo2;
                sheet.ColumnHeaders[(int)SHEET_COL.HINMEI_INV].Caption = Resources.TehaiMeisai_InvName;
                sheet.ColumnHeaders[(int)SHEET_COL.TEHAI_QTY].Caption = Resources.TehaiMeisai_ArrangementQty;
                sheet.ColumnHeaders[(int)SHEET_COL.QUANTITY_UNIT].Caption = Resources.TehaiMeisai_QtyUnit;
                sheet.ColumnHeaders[(int)SHEET_COL.TEHAI_FLAG].Caption = Resources.TehaiMeisai_ArrangementFlag;
                sheet.ColumnHeaders[(int)SHEET_COL.TEHAI_NO].Caption = Resources.TehaiMeisai_ArrangementNo;
                sheet.ColumnHeaders[(int)SHEET_COL.HENKYAKUHIN_FLAG].Caption = Resources.TehaiMeisai_HenkyakuhinFlag;
                sheet.ColumnHeaders[(int)SHEET_COL.HACCHU_QTY].Caption = Resources.TehaiMeisai_OrderQty;
                sheet.ColumnHeaders[(int)SHEET_COL.SHUKKA_QTY].Caption = Resources.TehaiMeisai_ShippingQty;
                sheet.ColumnHeaders[(int)SHEET_COL.ASSY_NO].Caption = Resources.TehaiMeisai_AssyNo;
                sheet.ColumnHeaders[(int)SHEET_COL.FREE1].Caption = Resources.TehaiMeisai_Free1;
                sheet.ColumnHeaders[(int)SHEET_COL.FREE2].Caption = Resources.TehaiMeisai_Free2;
                sheet.ColumnHeaders[(int)SHEET_COL.NOTE].Caption = Resources.TehaiMeisai_Note1;
                sheet.ColumnHeaders[(int)SHEET_COL.NOTE2].Caption = Resources.TehaiMeisai_Note2;
                sheet.ColumnHeaders[(int)SHEET_COL.NOTE3].Caption = Resources.TehaiMeisai_Note3;
                sheet.ColumnHeaders[(int)SHEET_COL.CUSTOMS_STATUS].Caption = Resources.TehaiMeisai_CustomsStatus;
                sheet.ColumnHeaders[(int)SHEET_COL.MAKER].Caption = Resources.TehaiMeisai_Maker;
                sheet.ColumnHeaders[(int)SHEET_COL.UNIT_PRICE].Caption = Resources.TehaiMeisai_UnitPrice;
                sheet.ColumnHeaders[(int)SHEET_COL.PHOTO].Caption = Resources.TehaiMeisai_Photo;
                sheet.ColumnHeaders[(int)SHEET_COL.TEHAI_RIREKI].Caption = Resources.TehaiMeisai_TehaiRireki;
                sheet.ColumnHeaders[(int)SHEET_COL.CREATE_USER_NAME].Caption = Resources.TehaiMeisai_CreateUserName;
                sheet.ColumnHeaders[(int)SHEET_COL.CREATE_DATE].Caption = Resources.TehaiMeisai_CreateDate;
#if __DEBUG_ENABLE_CODE__
                // デバッグ時は隠しも表示
                for (int index = 0; index < sheet.Columns.Count; index++)
                {
                    sheet.Columns[index].Hidden = false;
                }

                // handler登録
                this.setDebugEventtHandler();
#endif

                // 設定ファイルからグリッド幅を読み込む

                this.LoadGridSetting(this.shtMeisai, this._iniSectionPrefix);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア
        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            //base.DisplayClear();
            try
            {
                this.SearchCondClear();
                this.SearchResultClear();
                this.rdoInsert.Focus();

                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件のクリア
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SearchCondClear()
        {
            this.rdoInsert.Checked = true;
            txtEcsQuota.Text = string.Empty;
            txtEcsNo.Text = string.Empty;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索結果のクリア
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SearchResultClear()
        {
            this.cboBukkenName.ResetText();
            this.cboBukkenName.DataSource = null;
            this.txtSeiban.Text = string.Empty;
            this.txtCode.Text = string.Empty;
            this.cboKishu.ResetText();
            this.cboKishu.DataSource = null;
            this.txtARNo.Text = string.Empty;
            // グリッドクリア
            this.shtMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtMeisai.MaxRows)
            {
                this.shtMeisai.TopLeft = new Position(SHEET_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
            }
            this.shtMeisai.MultiRowAllClear();
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>S.Furugo 2018/11/05</create>
        /// <update>J.Chen 2022/05/18 STEP14</update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            if (!base.CheckInputSearch())
                return false;
            try
            {
                // 検索用入力チェック
                if (string.IsNullOrEmpty(this.txtEcsQuota.Text))
                {
                    // {0:期}を入力して下さい。
                    this.ShowMessage("T0100010001", this.lblQuota.Text);
                    this.txtEcsQuota.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(this.txtEcsNo.Text))
                {
                    // {0:ECS No.}を入力して下さい。
                    this.ShowMessage("T0100010001", this.lblEcsNo.Text);
                    this.txtEcsNo.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 編集用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>S.Furugo 2018/11/07</create>
        /// <update>T.Nukaga 2021/04/14 機種バイト数チェック追加</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // 技連情報入力チェック
                if (this.cboBukkenName.SelectedIndex < 0 || this.cboBukkenName.SelectedValue == null)   // 選択Only
                {
                    // {0:物件名}を選択して下さい。    
                    this.ShowMessage("T0100010001", this.lblBukkenName.Text);
                    this.cboBukkenName.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(this.txtSeiban.Text))
                {
                    // {0:製番}を入力して下さい。    
                    this.ShowMessage("T0100010001", this.lblSeiban.Text);
                    this.txtSeiban.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(this.txtCode.Text))
                {
                    // {0:CODE}を入力して下さい。
                    this.ShowMessage("T0100010001", this.lblCode.Text);
                    this.txtCode.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(this.cboKishu.Text))   // 選択 or 直入力
                {
                    // {0:機種}を入力して下さい。
                    this.ShowMessage("T0100010001", this.lblKishu.Text);
                    this.cboKishu.Focus();
                    return false;
                }
                // 機種
                if ((string.IsNullOrEmpty(this.cboKishu.Text) == false) && (Encoding.GetEncoding("Shift-JIS").GetByteCount(this.cboKishu.Text) > ComDefine.MAX_BYTE_LENGTH_KISHU))
                {
                    // {0}には{1}Byte以下で入力して下さい。
                    string[] param = new string[] { this.lblKishu.Text, ComDefine.MAX_BYTE_LENGTH_KISHU.ToString() };
                    this.ShowMessage("A9999999052", param);
                    return false;
                }

                // 明細情報入力チェック
                DisplayMode mode = DisplayMode.Initialize;
                if (rdoInsert.Checked) mode = DisplayMode.Insert;
                else if (rdoChange.Checked) mode = DisplayMode.Update;
                else if (rdoAllDelete.Checked) mode = DisplayMode.Delete;

                ret = this.CheckInputMeisaiData(mode, this.shtMeisai);
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
        /// 明細入力チェック(テーブル)
        /// </summary>
        /// <param name="mode">表示モード</param>
        /// <param name="sheet">シート</param>
        /// <returns>true:OK/false:NG</returns>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputMeisaiData(DisplayMode mode, Sheet sheet)
        {
            bool ret = true;
            try
            {
                int rowCnt = (mode == DisplayMode.Delete) ? sheet.Rows.Count : sheet.Rows.Count - 1;    // 削除モード時以外は新規行分を減算する
                for (int i = 0; i < rowCnt; i++)
                {
                    ret = this.CheckInputMeisaiData(mode, sheet, i);
                    if (!ret)
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
        /// 明細入力チェック(行)
        /// </summary>
        /// <param name="mode">表示モード</param>
        /// <param name="sheet">シート</param>
        /// <param name="row">行</param>
        /// <returns>true:OK/false:NG</returns>
        /// <create>D.Naito 2018/11/28</create>
        /// <update>J.Chen 2023/02/21 半角英数字記号チェック追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputMeisaiData(DisplayMode mode, Sheet sheet, int row)
        {
            bool ret = true;
            try
            {
                // 入力チェック
                switch (mode)
                {
                    case DisplayMode.Insert:
                    case DisplayMode.Update:
                        foreach (CheckColumn item in _inputCheckColumns)
                        {
                            switch (item.pattern)
                            {
                                case 1: // 必須
                                    if (item.col == (int)SHEET_COL.ASSY_NO)
                                    {
                                        // 手配区分「ASSY」の場合、Assy no.は必須入力
                                        string selectedFlag = sheet[(int)SHEET_COL.TEHAI_FLAG, row].Value == null ? null : sheet[(int)SHEET_COL.TEHAI_FLAG, row].Value.ToString();
                                        if (TEHAI_FLAG.ASSY_VALUE1.Equals(selectedFlag) && string.IsNullOrEmpty(Convert.ToString(sheet[item.col, row].Value)))
                                        {
                                            // {0}行目の{1}を入力して下さい。
                                            this.ShowMessage("T0100010005", (row + 1).ToString(), item.label);
                                            this.shtMeisai.ActivePosition = new Position(item.col, row);
                                            this.shtMeisai.Focus();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(Convert.ToString(sheet[item.col, row].Value)))
                                        {
                                            // {0}行目の{1}を入力して下さい。
                                            this.ShowMessage("T0100010005", (row + 1).ToString(), item.label);
                                            this.shtMeisai.ActivePosition = new Position(item.col, row);
                                            this.shtMeisai.Focus();
                                            return false;
                                        }
                                    }
                                    break;
                                case 2: // 必須 + (数(手配) < 1)
                                    if (DSWUtil.UtilConvert.ToDecimal(sheet[item.col, row].Value, 0m) < 1m)
                                    {
                                        // {0}行目の{1}を{2}以上の値で入力して下さい。
                                        this.ShowMessage("T0100010007", (row + 1).ToString(), item.label, "1");
                                        this.shtMeisai.ActivePosition = new Position(item.col, row);
                                        this.shtMeisai.Focus();
                                        return false;
                                    }
                                    break;
                                case 3: // 手配区分 == 発注の場合、必須 + (数(発注)  < 入荷数)

                                    if (TEHAI_FLAG.ORDERED_VALUE1.Equals(Convert.ToString(sheet[(int)SHEET_COL.TEHAI_FLAG, row].Value)))
                                    {
                                        decimal arrival_qty = DSWUtil.UtilConvert.ToDecimal(sheet[(int)SHEET_COL.ARRIVAL_QTY, row].Value, 0m);
                                        if (string.IsNullOrEmpty(Convert.ToString(sheet[item.col, row].Value))
                                            || DSWUtil.UtilConvert.ToDecimal(sheet[item.col, row].Value) < arrival_qty)
                                        {
                                            // {0}行目の{1}を{2}以上の値で入力して下さい。
                                            this.ShowMessage("T0100010007", (row + 1).ToString(), item.label, arrival_qty.ToString());
                                            this.shtMeisai.ActivePosition = new Position(item.col, row);
                                            this.shtMeisai.Focus();
                                            return false;
                                        }
                                    }
                                    break;
                                // DB登録時にチェックタイミング変更
                                //case 4: // 必須 + (数(出荷) < 出荷明細数量(合計))
                                //    decimal num = DSWUtil.UtilConvert.ToDecimal(sheet[(int)SHEET_COL.SHUKKA_MEISAI_QTY, row].Value, 0m);
                                //    if (string.IsNullOrEmpty(Convert.ToString(sheet[item.col, row].Value))
                                //        ||  DSWUtil.UtilConvert.ToDecimal(sheet[item.col, row].Value) < num)
                                //    {
                                //        // {0}行目の{1}を{2}以上の値で入力して下さい。
                                //        this.ShowMessage("T0100010007", (row + 1).ToString(), item.label, num.ToString());
                                //        this.shtMeisai.ActivePosition = new Position(item.col, row);
                                //        this.shtMeisai.Focus();
                                //        return false;
                                //    }
                                //    break;
                                case 5: // 必須 + free2フォーマット
                                    if (string.IsNullOrEmpty(Convert.ToString(sheet[item.col, row].Value))
                                        || !System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(sheet[item.col, row].Value), @"^MADE IN \w+$"))
                                    {
                                        // {0}行目の{1}を'MADE IN xxx'の形式で入力して下さい。
                                        this.ShowMessage("T0100010006", (row + 1).ToString(), item.label);
                                        this.shtMeisai.ActivePosition = new Position(item.col, row);
                                        this.shtMeisai.Focus();
                                        return false;
                                    }
                                    break;
                                case 6: // 必須 + 半角英数字
                                    if (string.IsNullOrEmpty(Convert.ToString(sheet[item.col, row].Value))
                                        || !System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(sheet[item.col, row].Value), @"^[ -~]+$"))
                                    {
                                        // {0}行目の{1}を半角英数字記号のみ入力してください。
                                        this.ShowMessage("T0100010041", (row + 1).ToString(), item.label);
                                        this.shtMeisai.ActivePosition = new Position(item.col, row);
                                        this.shtMeisai.Focus();
                                        return false;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case DisplayMode.Delete:
                        if (DSWUtil.UtilConvert.ToDecimal(sheet[(int)SHEET_COL.SHUKKA_MEISAI_CNT, row].Value, 0m) > 0m)
                        {
                            // 行削除を行う場合は、TAG連携済み行を含めないようにして下さい。
                            this.ShowMessage("T0100010017");
                            this.shtMeisai.Focus();
                            this.shtMeisai.ActivePosition = new Position(0, row);  // 非表示項目なため、列のTOPへ
                            return false;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(sheet[(int)SHEET_COL.ESTIMATE_NO, row].Value)))
                        {
                            // 行削除を行う場合は、見積済み行を含めないようにして下さい。
                            this.ShowMessage("T0100010018");
                            this.shtMeisai.ActivePosition = new Position(0, row);  // 非表示項目なため、列のTOPへ
                            this.shtMeisai.Focus();
                            return false;
                        }
                        break;
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
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunSearch(Position pos)
        {
            var result = this.RunSearch();
            if (result)
            {
                // 検索が成功し、件数が所定以上なら、位置を復元する
                if (this.shtMeisai.Rows.Count > pos.Row)
                {
                    this.shtMeisai.TopLeft = pos;
                }
            }
            return result;
        }
        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>S.Furugo 2018/10/26</create>
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
        /// <create>S.Furugo 2018/10/26</create>
        /// <update>D.Okumura 2019/02/13 2回連続して登録を行うと機種が異なる値となる不具合を修正</update>
        /// <update>H.Iimuro 2022/10/25 背景色表示不具合を修正</update>
        /// <update>J.Chen 2023/12/21 金額変更チェック用データテーブル追加</update>
        /// <update>J.Chen 2024/11/06 履歴更新比較用dt追加</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondT01 cond = this.GetCondition();
                ConnT01 conn = new ConnT01();
                DataSet ds = conn.GetGiren(cond);

                this.SuspendLayout();
                this.shtMeisai.Redraw = false;
                // ----- 新規 -----
                if (rdoInsert.Checked)
                {
                    if (ComFunc.IsExistsData(ds, Def_M_ECS.Name))
                    {
                        // 手配明細情報が既に登録されています。
                        this.ShowMessage("T0100010003");
                        return false;
                    }

                    this.shtMeisai.AllowUserToAddRows = true;

                    //コンボボックスセット
                    this.SetComboBox();
                    this.cboBukkenName.SelectedIndex = -1;  // 未選択
                    this.cboKishu.SelectedIndex = -1;       // 未選択

                    DataTable dt = this.GetSchemaTehaiMeisai();
                    this.shtMeisai.DataSource = dt;

                    // モード切り替え
                    this.ChangeMode(DisplayMode.Insert);
                }
                // ----- 変更・削除 -----
                else if (rdoChange.Checked || rdoAllDelete.Checked)
                {

                    if (!ComFunc.IsExistsData(ds, Def_M_ECS.Name))
                    {
                        // 手配明細情報がありません。
                        this.ShowMessage("T0100010004");
                        return false;
                    }

                    DataSet dsTehaiMeisai = conn.GetTehaiMeisai(cond);
                    DataTable dtTehaiMeisai = dsTehaiMeisai.Tables[Def_T_TEHAI_MEISAI.Name];

                    this.shtMeisai.AllowUserToAddRows = true;

                    this.SetComboBox();

                    this.shtMeisai.DataSource = dtTehaiMeisai;
                    dtTemp = dtTehaiMeisai.Copy();
                    this.tempTable = dtTehaiMeisai.Copy();

                    DataTable dtEcs = ds.Tables[Def_M_ECS.Name];
                    var arNo = ComFunc.GetFld(dtEcs, 0, Def_M_ECS.AR_NO);
                    this.cboBukkenName.SelectedValue = ComFunc.GetFld(dtEcs, 0, Def_M_ECS.PROJECT_NO);
                    this.txtSeiban.Text = ComFunc.GetFld(dtEcs, 0, Def_M_ECS.SEIBAN);
                    this.txtCode.Text = ComFunc.GetFld(dtEcs, 0, Def_M_ECS.CODE);   // 直値のためTextに設定

                    // コンボボックスが選択状態(データソースを再設定した際に、最初の項目が選択されている)のとき、
                    // Textプロパティを設定しても、画面表示は変化するが、
                    // Textプロパティは変化しないため、一度未選択にしてからTextプロパティをセットする
                    this.cboKishu.SelectedIndex = -1;       // 未選択
                    this.cboKishu.Text = ComFunc.GetFld(dtEcs, 0, Def_M_ECS.KISHU);

                    this.txtARNo.Text = string.IsNullOrEmpty(arNo) ? "" : arNo.Remove(0, lblAR.Text.Length).Trim();
                    this._girenVerOrg = ComFunc.GetFldToDateTime(dtEcs, 0, Def_M_ECS.VERSION);

                    // モード切り替え
                    this.ChangeMode(rdoChange.Checked ? DisplayMode.Update : DisplayMode.Delete,
                        ComFunc.GetFld(dtEcs, 0, Def_M_ECS.KANRI_FLAG).Equals(KANRI_FLAG.KANRYO_VALUE1));

                    if (1 < this.shtMeisai.MaxRows)
                    {
                        // 背景色更新処理
                        this.SetAllCheck(false);
                    }
                }

                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }

                _updCheckDt = (this.shtMeisai.DataSource as DataTable).Copy();

                this.shtMeisai.Focus();
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.shtMeisai.Redraw = true;
                this.ResumeLayout();
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
        /// <create>S.Furugo 2018/10/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            try
            {
                bool ret = base.RunEdit();

                // 保存後の再検索
                if (ret)
                {
                    // グリッドクリア
                    //this.SheetClear();
                    // 再検索
                    //this.RunSearch();

                    // 手配見積を起動
                    if (estimateNoList != null)
                    {
                        foreach (var estimateNo in estimateNoList)
                        {
                            // 手配見積画面(編集)を開く
                            using (var frm = new TehaiMitsumori(this.UserInfo, null, null, ComDefine.TITLE_T0100040))
                            {
                                frm.StartEditForTehaiMeisai(estimateNo);
                                frm.ShowDialog(this);
                            }
                        }
                    }

                    if (this._dialogMode)
                    {
                        // 終了時のメッセージを表示しない
                        this.IsChangedCloseQuestion = false;
                        this.IsCloseQuestion = false;
                        // 外部連携モードは、登録完了で終了
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        // 新規登録時の再検索は変更で再検索する
                        if (this.rdoInsert.Checked)
                            this.rdoChange.Checked = true;

                        // 削除または、再検索が失敗したときは初期状態へ遷移する
                        if (this.rdoAllDelete.Checked || !this.RunSearchExec())
                        {
                            this.DisplayClear();
                            this.ChangeMode(DisplayMode.Initialize);
                            this.rdoInsert.Focus();
                        }
                    }
                }
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
        /// 新規処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>D.Naito 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                // 更新データテーブル作成
                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                DataTable dtGiren = this.GetDataGiren();

                if (dt == null || dtGiren == null) return false;

                // 表示順番設定
                for (int i = 0, j = 1; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].RowState != DataRowState.Deleted)
                    {
                        dt.Rows[i][Def_T_TEHAI_MEISAI.DISP_NO] = i + j;
                    }
                    else
                    {
                        j--;
                    }
                }

                foreach (DataRow r in dt.Rows)
                {
                    if (string.IsNullOrEmpty(r[Def_T_TEHAI_MEISAI.SHUKKA_QTY].ToString()))
                    {
                        r[Def_T_TEHAI_MEISAI.SHUKKA_QTY] = "0";
                    }
                }

                DataSet ds = new DataSet();
                setColumnsField(dt, Def_T_TEHAI_MEISAI.ECS_QUOTA, txtEcsQuota.Text);
                setColumnsField(dt, Def_T_TEHAI_MEISAI.ECS_NO, txtEcsNo.Text);
                ds.Tables.Add(dt);
                ds.Tables.Add(dtGiren);

                // DB更新
                CondT01 cond = this.GetCondition();
                ConnT01 conn = new ConnT01();
                string errMsgID;
                string[] args;
                if (!conn.InsTehaiMeisai(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                    }
                    return false;
                }
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
        /// <create>D.Naito 2018/11/26</create>
        /// <update>J.Chen 2022/05/18 STEP14</update>
        /// <update>J.Chen 2022/10/31 手配Noチェック</update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                // 更新用データテーブル作成
                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                DataTable dtGiren = this.GetDataGiren();

                // 表示順番設定
                for (int i = 0, j = 1; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].RowState != DataRowState.Deleted)
                    {
                        dt.Rows[i][Def_T_TEHAI_MEISAI.DISP_NO] = i + j;
                    }
                    else
                    {
                        j--;
                    }
                }
                this.shtMeisai.DataSource = dt;

                if (dt == null || dtGiren == null) return false;

                foreach (DataRow r in dt.Rows)
                {
                    if (r.RowState == DataRowState.Deleted)
                        continue;
                    if (string.IsNullOrEmpty(r[Def_T_TEHAI_SKS.TEHAI_NO].ToString()))
                    {
                        r[Def_T_TEHAI_MEISAI.TEHAI_KIND_FLAG] = TEHAI_KIND_FLAG.NONE_VALUE1;
                    }
                    if (string.IsNullOrEmpty(r[Def_T_TEHAI_MEISAI.SHUKKA_QTY].ToString()))
                    {
                        r[Def_T_TEHAI_MEISAI.SHUKKA_QTY] = "0";
                    }
                }

                DataTable dtInsert = this.GetDataTehaiMeisaiFilter(dt, DataRowState.Added);      // 追加データ抽出
                DataTable dtUpdate = this.GetDataTehaiMeisaiFilter(dt, DataRowState.Modified);   // 更新データ抽出
                DataTable dtDelete = this.GetDataTehaiMeisaiFilter(dt, DataRowState.Deleted);    // 削除データ抽出

                DataSet ds = new DataSet();
                if (dtInsert != null && dtInsert.Rows.Count > 0)
                {
                    ds.Tables.Add(dtInsert);
                }
                if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                {
                    ds.Tables.Add(dtUpdate);
                }
                if (dtDelete != null && dtDelete.Rows.Count > 0)
                {
                    ds.Tables.Add(dtDelete);
                }
                ds.Tables.Add(dtGiren);

                // DB更新
                CondT01 cond = this.GetCondition();
                ConnT01 conn = new ConnT01();
                string errMsgID;
                string[] args;

                DataSet dsTehaiMeisai = conn.GetTehaiMeisai(cond);
                // 変更前のデータ
                DataTable dtTehaiMeisaiBefore = dsTehaiMeisai.Tables[Def_T_TEHAI_MEISAI.Name];

                CheckTehaiMeisaiSKS(dtTehaiMeisaiBefore, dt, ds);

                if (!conn.UpdTehaiMeisai(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                    }
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
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

        #region 削除処理
        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>S.Furugo 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                // 削除用データテーブル作成
                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                DataTable dtGiren = this.GetDataGiren();

                if (dt == null || dtGiren == null) return false;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ds.Tables.Add(dtGiren);

                // DB削除
                CondT01 cond = this.GetCondition();
                ConnT01 conn = new ConnT01();
                string errMsgID;
                string[] args;
                if (!conn.DelTehaiMeisai(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                    }
                    return false;
                }

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
        /// F1ボタンクリック(保存ボタン)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/10/23</create>
        /// <update>J.Chen 2023/12/22 金額影響チェック、手配見積画面開く</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            // 見積金額に影響あるかどうか
            IsEstimatedAmountChanged();

            base.fbrFunction_F01Button_Click(sender, e);
            try
            {

            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F2ボタンクリック(明細分割)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/07</create>
        /// <update>J.Chen 2022/05/18 STEP14</update>
        /// <update>H.Iimuro 2022/10/19 チェックボックス選択数のエラー追加</update>
        /// <update>J.Chen 2023/10/13 明細分割時、返却フラグ変更による出荷数の修正補助を行わないようにする</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F02Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                // チェック数確認
                if (this.CheckCount() <= 0)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("T0100060004");
                    return;
                }

                if (rdoAllDelete.Checked || !this.shtMeisai.AllowUserToAddRows) return;

                //１件以上選択
                if (CheckCount() > 1)
                {
                    // 
                    this.ShowMessage("T0100010032");
                    return;
                }

                ArrayList rows = CheckIndex();
                int row = (int)rows[0];
                // 新規行の場合、なにもしない
                if (this.shtMeisai.Rows.Count <= 1 || (this.shtMeisai.Rows.Count - 1) == row)
                {
                    // 明細分割を行う場合は、最終行を含まないようにして下さい。
                    this.ShowMessage("T0100010011");
                    return;
                }

                // 状態チェック
                //if (!TEHAI_FLAG.ORDERED_VALUE1.Equals(Convert.ToString(this.shtMeisai[(int)SHEET_COL.TEHAI_FLAG, row].Value)))
                //{
                //    // 選択行は発注以外のため、分割できません。
                //    this.ShowMessage("T0100010010");
                //    return;
                //}

                if (DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[(int)SHEET_COL.SHUKKA_MEISAI_CNT, row].Value, 0m) > 0m)
                {
                    // 選択行はTAG連携済みのため、分割できません。
                    this.ShowMessage("T0100010013");
                    return;
                }

                decimal tehaiQty = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[(int)SHEET_COL.TEHAI_QTY, row].Value, 0m);
                if (tehaiQty <= 1m)
                {
                    // 選択行はこれ以上、分割できません。
                    this.ShowMessage("T0100010012");
                    return;
                }

                decimal hacchuQty = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[(int)SHEET_COL.HACCHU_QTY, row].Value, 0m);
                decimal shukkaQty = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[(int)SHEET_COL.SHUKKA_QTY, row].Value, 0m);
                decimal arrivalQty = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[(int)SHEET_COL.ARRIVAL_QTY, row].Value, 0m);
                decimal assyQty = DSWUtil.UtilConvert.ToDecimal(this.shtMeisai[(int)SHEET_COL.ASSY_QTY, row].Value, 0m);
                using (TehaiMeisaiBunkatsu frm = new TehaiMeisaiBunkatsu(this.UserInfo, Convert.ToString(this.shtMeisai[(int)SHEET_COL.TEHAI_FLAG, row].Value), new TehaiMeisaiBunkatsu.Qty()
                {
                    Tehai = tehaiQty,
                    Hacchu = hacchuQty,
                    Shukka = shukkaQty,
                    Arrival = arrivalQty,
                    Assy = assyQty
                }))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        int add_row = row + 1;
                        this.shtMeisai.Redraw = false;
                        this.shtMeisai.InsertRow(add_row, false);

                        // 元
                        this.shtMeisai[(int)SHEET_COL.TEHAI_QTY, row].Value = frm.Src.Tehai;
                        this.shtMeisai[(int)SHEET_COL.HACCHU_QTY, row].Value = frm.Src.Hacchu;
                        this.shtMeisai[(int)SHEET_COL.SHUKKA_QTY, row].Value = frm.Src.Shukka;
                        this.shtMeisai[(int)SHEET_COL.ARRIVAL_QTY, row].Value = frm.Src.Arrival;
                        this.shtMeisai[(int)SHEET_COL.ASSY_QTY, row].Value = frm.Src.Assy;

                        // 先
                        this.shtMeisai[(int)SHEET_COL.TEHAI_QTY, add_row].Value = frm.Dst.Tehai;
                        this.shtMeisai[(int)SHEET_COL.HACCHU_QTY, add_row].Value = frm.Dst.Hacchu;
                        this.shtMeisai[(int)SHEET_COL.SHUKKA_QTY, add_row].Value = frm.Dst.Shukka;
                        this.shtMeisai[(int)SHEET_COL.ARRIVAL_QTY, add_row].Value = frm.Dst.Arrival;
                        this.shtMeisai[(int)SHEET_COL.ASSY_QTY, add_row].Value = frm.Dst.Assy;

                        this.shtMeisai[(int)SHEET_COL.TEHAI_KIND_FLAG, add_row].Value = TEHAI_KIND_FLAG.NONE_VALUE1;

                        isMeisaiBunkatsu = true;
                        this.copyCell(this.shtMeisai, add_row, _copyBunkatuColums); // 行コピー
                        isMeisaiBunkatsu = false;

                        this.shtMeisai.ActivePosition = new Position(this.shtMeisai.ActivePosition.Column, add_row);
                        this.shtMeisai.Redraw = true;
                        this.shtMeisai.Focus();

                        // 行間に新しいデータを追加する場合、データテーブルに更新する
                        DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                        Object[] rowArray = dt.Rows[dt.Rows.Count - 1].ItemArray;
                        DataRow rowAdd = dt.NewRow();
                        rowAdd.ItemArray = rowArray;
                        dt.Rows.RemoveAt(dt.Rows.Count - 1);
                        dt.Rows.InsertAt(rowAdd, add_row);
                        this.shtMeisai.DataSource = dt;
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
        /// F3ボタンクリック(削除)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/10/23</create>
        /// <update>K.Tsutsumi 2019/03/08 出荷側に合わせて複数行削除対応</update>
        /// <update>H.Iimuro 2022/10/19 チェックボックス選択数のエラー追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                // チェック数確認
                if (this.CheckCount() <= 0)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("T0100060004");
                    return;
                }
                // 削除モードの場合は処理を抜ける
                if (rdoAllDelete.Checked) return;
                // 行がない場合は処理を抜ける
                if (this.shtMeisai.Rows.Count < 1) return;

                //選択されているセル範囲をすべて取得する
                ArrayList objRanges = CheckIndex();

                foreach (int range in objRanges)
                {
                    // 行追加時の最終行の場合は処理を抜ける。
                    if ((this.shtMeisai.AllowUserToAddRows) &&
                       (this.shtMeisai.Rows.Count - 1 == range))
                    {
                        // 行削除を行う場合は、最終行を含まないようにして下さい。
                        this.ShowMessage("T0100010014");
                        return;
                    }

                    // 入力チェック
                    if (!this.CheckInputMeisaiData(DisplayMode.Delete, this.shtMeisai, range))
                        return;
                }

                // 選択行を削除してもよろしいですか？
                if (this.ShowMessage("T0100010022") == DialogResult.OK)
                {

                    this.shtMeisai.Redraw = false;

                    for (int i = objRanges.Count - 1; i >= 0; i--)
                    {
                        //行削除
                        this.shtMeisai.RemoveRow((int)objRanges[i], 1, false);
                    }

                    this.shtMeisai.Redraw = true;
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F4ボタンクリック(Copy)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/11/27</create>
        /// <update>H.Iimuro 2022/10/19 １行コピーに変更</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                // チェック数確認
                if (this.CheckCount() <= 0)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("T0100060004");
                    return;
                }
                // 全削除モードの場合は、処理を抜ける
                if (rdoAllDelete.Checked) return;
                // 行がない場合は処理を抜ける
                if (this.shtMeisai.Rows.Count < 1) return;

                //選択されているセル範囲をすべて取得する
                ArrayList objRanges = CheckIndex();

                foreach (int range in objRanges)
                {
                    // 行追加時の最終行の場合は処理を抜ける。
                    if ((this.shtMeisai.AllowUserToAddRows) &&
                       (this.shtMeisai.Rows.Count - 1 == range))
                    {
                        // 行Copyを行う場合は、最終行を含まないようにして下さい。
                        this.ShowMessage("T0100010023");
                        return;
                    }
                }


                this.shtMeisai.Redraw = false;
                var rowPos = this.shtMeisai.MaxRows - 1;
                // 行の追加
                this.shtMeisai.InsertRow(rowPos, objRanges.Count, false);

                // 行のコピー
                foreach(int row in objRanges)
                {
                    SetSheetRowInitData(this.shtMeisai, rowPos);
                    this.copyCell(this.shtMeisai, row, rowPos, _rowCopyColums);
                    rowPos++;
                }
                this.shtMeisai.Redraw = true;
                this.shtMeisai.Focus();
            }
            catch (Exception ex)
            {

                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F5ボタンクリック(項コピー)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/05</create>
        /// <update>H.Iimuro 2022/10/19 機能削除</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック(Clear)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/10/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                // 検索結果をClearします。\r\nよろしいですか？
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                this.SearchResultClear();
                this.ChangeMode(DisplayMode.Initialize);
                this.txtEcsQuota.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック(All Clear)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/10/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // 画面をクリアします。\r\nよろしいですか？
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F8変更履歴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F11Button_Click(sender, e);

            var conn = new ConnT01();
            var cond = new CondT01(this.UserInfo);

            var ecsQuota = this.txtEcsQuota.Text;
            var ecsNo = this.txtEcsNo.Text;

            using (var f = new TehaiMeisaiRireki(this.UserInfo, ecsQuota, ecsNo))
            {
                f.ShowDialog();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F9ボタンクリック(手配取込)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/05</create>
        /// <update>TW-Tsuji 2022/10/19
        ///        【Step15】手配取込Excel様式の拡張と変更</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F09Button_Click(sender, e);
            try
            {
                using (var frm = new TehaiMeisaiListImport(this.UserInfo))
                {
                    //【Step15】手配取込Excel様式の拡張と変更 2022/10/19（TW-Tsuji）
                    //
                    //　明細取込フォームに、更新かどうかのフラグを渡す
                    if (this.EditMode == SystemBase.EditMode.None)
                    {
                        // ラジオボタン 新規
                        if (this.rdoInsert.Checked)
                        {
                            frm.ARG_IsUpdate = false;
                        }
                        else
                        {
                            // 指定された処理選択では、実行できません。
                            this.ShowMessage("T0100010034");
                            return;
                        }
                    }
                    else if (this.EditMode == SystemBase.EditMode.Delete)
                    {
                        // 指定された処理選択では、実行できません。
                        this.ShowMessage("T0100010034");
                        return;
                    }
                    else
                    {
                        frm.ARG_IsUpdate = true;
                    }

                    //
                    //　画面に設定されている、期 と ECSNoを渡す
                    if (!String.IsNullOrEmpty(this.txtEcsQuota.Text))
                    {
                        frm.ARG_EcsQuota = Int32.Parse(txtEcsQuota.Text);
                        frm.ARG_EcsNo = this.txtEcsNo.Text;
                    }
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        //【Step15】手配取込Excel様式の拡張と変更 2022/10/19（TW-Tsuji）
                        //
                        //===Deleted
                        ////////　行を追加
                        //////AddXlsDataZubanKeishiki(this.shtMeisai, frm.Result.list);
                        //
                        //===Added
                        //
                        //　明細追加が正常終了した場合（明細行数が０件の場合も含む）
                        if (frm.Result.meisaiAdd == true)
                        {
                            //　新規登録の場合
                            if (frm.ARG_IsUpdate == false)
                            {
                                //Excelから読んだ、期とECS No.をセットする.
                                this.txtEcsQuota.Text = frm.Result.header.EcsQuota;
                                this.txtEcsNo.Text = frm.Result.header.EcsNo;
                                try
                                {
                                    this.ClearMessage();
                                    this.RunSearch();
                                    this.cboBukkenName.Focus();
                                }
                                catch (Exception ex)
                                {
                                    CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                                }
                            }

                            //　ヘッダデータおよびリストの取込
                            if (frm.Result.meisai != null && frm.Result.meisai.Rows.Count != 0)
                            {
                                AddXlsDataZubanKeishikiEx(this.shtMeisai, frm.Result.header, frm.Result.meisai);
                            }

                            // 手配明細List(Excel)を取り込みました。
                            this.ShowMessage("T0100010016");
                        }
                        //
                        //　明細行に何らかのエラーがあった場合
                        else
                        {
                            // 手配明細Excelのデータ取込を中止しました。
                            this.ShowMessage("T0100010036");
                        }
                        //
                        //===修正ここまで
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region 行コピペ
        #if __DEBUG_ENABLE_CODE__
        private const int SHEET_COL_COPY_START = (int)SHEET_COL.ECS_QUOTA;          // コピー開始列
        private const int SHEET_COL_COPY_END = (int)SHEET_COL.SHUKKA_MEISAI_QTY;    // コピー終了列
        /// --------------------------------------------------
        /// <summary>
        /// F9ボタンクリック(デバッグ用に行を選択行データコピペ)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F11Button_Click(object sender, EventArgs e)
        {
            var selectionType = this.shtMeisai.SelectionType;
            base.fbrFunction_F11Button_Click(sender, e);
            try
            {
                if (rdoAllDelete.Checked || !this.shtMeisai.AllowUserToAddRows) return;

                int row = this.shtMeisai.ActivePosition.Row;
                // 新規追加行の場合
                if (this.shtMeisai.Rows.Count <= 1 || (this.shtMeisai.Rows.Count - 1) <= row)   return;

                this.shtMeisai.Redraw = false;
                // 行コピー
                this.shtMeisai.SelectionType = SelectionType.Range;
                this.shtMeisai.UIReSelection(new Range(SHEET_COL_COPY_START, row, SHEET_COL_COPY_END, row));
                this.shtMeisai.UICopy();

                // 行ペースト
                row++;
                this.shtMeisai.InsertRow(row, false);
                this.shtMeisai.UIReSelection(new Range(SHEET_COL_COPY_START, row, SHEET_COL_COPY_END, row));
                this.shtMeisai.UIPaste();
                Clipboard.Clear();

                this.shtMeisai.ActivePosition = new Position(this.shtMeisai.ActivePosition.Column, row);
                this.shtMeisai.Redraw = true;
                this.shtMeisai.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.SelectionType = selectionType;
            }
        }
#endif
        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック(Team切替)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Iimuro 2022/10/19　F10追加</create>
        /// <update>H.Iimuro 2022/10/19　未指定の背景色 変更</update>
        /// <update>J.Chen 2022/11/14　見積済み・出荷済み制限追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                // チェック数確認
                if (this.CheckCount() <= 0)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("T0100060004");
                    return;
                }

                // 選択行の見積状態を変更してもよろしいですか？
                if (this.ShowMessage("T0100010033") != DialogResult.OK)
                {
                    return;
                }

                this.shtMeisai.Redraw = false;
                for (int i = 0; i < this.shtMeisai.MaxRows; i++)
                {
                    // 未選択はスキップ
                    var checkstate = this.shtMeisai[SHEET_COL_CHECK, i].Value;
                    if (checkstate == null || checkstate.ToString() != CHECK_ON_STR)
                        continue;

                    // 更新用情報を追加
                    if (this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value == null)
                    {
                        this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value = ComDefine.NEUTRAL_FLAG;
                    }
                    var checkEstimateFlag = this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value.ToString();

                    //有償 → 未設定
                    if (checkEstimateFlag == ComDefine.ONEROUS_FLAG)
                    {
                        EstimateStatus = EstimateMode.Neutral;
                    }
                    //無償 → 有償
                    if (checkEstimateFlag == ComDefine.GRATIS_FLAG)
                    {
                        EstimateStatus = EstimateMode.Onerous;
                    }
                    //未設定 → 無償
                    if (checkEstimateFlag == ComDefine.NEUTRAL_FLAG)
                    {
                        EstimateStatus = EstimateMode.Gratis;
                    }
                    var lineName = (i + 1).ToString();

                    if (!string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_TEHAI_RENKEI_NO, i].Text))
                    {
                        switch (EstimateStatus)
                        {
                            case EstimateMode.Onerous: //無償 → 有償
                                // 無償でかつ、出荷済み
                                if (this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value as string == ESTIMATE_FLAG.GRATIS_VALUE1
                                    && int.Parse(this.shtMeisai[SHEET_COL_CNT, i].Text) >= 1)
                                {
                                    // {0}行目に有償/無償が"無償"かつ出荷済み明細を含んでいます。
                                    this.ShowMessage("T0100050005", lineName);
                                    return;
                                }
                                break;
                            case EstimateMode.Neutral: //有償 → 未設定
                                // 有償でかつ、出荷済み
                                if (this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Text == ESTIMATE_FLAG.ONEROUS_VALUE1
                                    && int.Parse(this.shtMeisai[SHEET_COL_CNT, i].Text) >= 1)
                                {
                                    // {0}行目に有償/無償が"有償"かつ出荷済み明細を含んでいます。
                                    this.ShowMessage("T0100050007", lineName);
                                    return;
                                }
                                // 有償でかつ、出荷済み
                                if (this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Text == ESTIMATE_FLAG.ONEROUS_VALUE1
                                    && !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_ESTIMATE_NO, i].Text))
                                {
                                    // {0}行目に有償/無償が"有償"かつ見積Noがセットされている明細があります。
                                    this.ShowMessage("T0100050006", lineName);
                                    return;
                                }
                                break;
                            case EstimateMode.Gratis: //未設定 → 無償
                                break;
                            default:
                                //ありえない
                                return;
                        }
                    }

                    // 背景色を未設定に
                    if (checkEstimateFlag == ComDefine.ONEROUS_FLAG)
                    {
                        checkEstimateFlag = ComDefine.NEUTRAL_FLAG;
                        //this.shtMeisai.Rows[i].BackColor = Color.Empty;
                        //this.shtMeisai.Rows[i].ForeColor = Color.Empty;
                        //this.shtMeisai.Rows[i].DisabledBackColor = Color.FromArgb(223, 223, 223); ;
                        //this.shtMeisai.Rows[i].DisabledForeColor = Color.Black;
                        defaultRowBackColor(this.shtMeisai.Rows[i]);

                        this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value = checkEstimateFlag;
                        this.shtMeisai[ESTIMATE_COLOR, i].Value = ComDefine.NEUTRAL_COLOR;

                        continue;
                    }

                    var color = ComDefine.GRATIS_COLOR;
                    if (checkEstimateFlag == ComDefine.GRATIS_FLAG)
                    {
                        checkEstimateFlag = ComDefine.ONEROUS_FLAG;
                        color = ComDefine.ONEROUS_COLOR;
                    }
                    else if (checkEstimateFlag == ComDefine.NEUTRAL_FLAG)
                    {
                        checkEstimateFlag = ComDefine.GRATIS_FLAG;
                        color = ComDefine.GRATIS_COLOR;
                    }

                    this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value = checkEstimateFlag;
                    this.shtMeisai[ESTIMATE_COLOR, i].Value = color;
                    SetupRowColor(this.shtMeisai.Rows[i], color, this.shtMeisai.GridLine, Borders.All);
                }

                this.shtMeisai.Redraw = true;


            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F11ボタンクリック（単価計算）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Shioshi 2022/04/11</create>
        /// <update>J.Chen 2022/06/10 STEP14 Assy子存在しない場合エラーメッセージ表示</update>
        /// <update>H.Iimuro 2022/10/19 チェックボックス選択数のエラー追加</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F11Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F11Button_Click(sender, e);
            try
            {
                this.ClearMessage();
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
                    this.ShowMessage("T0100010031");
                    return;
                }
                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();

                int row = (int)CheckIndex()[0];
                if (!String.IsNullOrEmpty(Convert.ToString(this.shtMeisai[(int)SHEET_COL.ASSY_NO, row].Value))
                    && this.shtMeisai[(int)SHEET_COL.TEHAI_FLAG, row].Value.ToString() == TEHAI_FLAG.ASSY_VALUE1 )
                {
                    decimal total = 0;
                    decimal result = 0;
                    decimal ratio = decimal.Parse(this.UserInfo.SysInfo.calculationRate);
                    decimal oya_qty = decimal.Parse(this.shtMeisai[(int)SHEET_COL.TEHAI_QTY, row].Value.ToString());
                    
                    var assy_no = this.shtMeisai[(int)SHEET_COL.ASSY_NO, row].Value.ToString();

                    foreach (DataRow dr in dt.Rows)
                    {
                        var tehaiFlag = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_FLAG, null);
                        if (tehaiFlag == TEHAI_FLAG.ASSY_VALUE1) continue;
                        var assyno = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.ASSY_NO, null);
                        if (assyno == assy_no)
                        {
                            var tehaiQty = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_QTY, null);
                            var unitPrice = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.UNIT_PRICE, null);
                            if (!String.IsNullOrEmpty(tehaiQty) && !String.IsNullOrEmpty(unitPrice))
                            {
                                total += (decimal.Parse(tehaiQty) * decimal.Parse(unitPrice));
                            }
                        }
                    }

                    if (total != 0 && oya_qty != 0)
                    {
                        result = total * ratio / oya_qty;
                        // ｘ行目の単価計算完了しました。
                        this.ShowMessage("T0100010030", (row + 1).ToString());
                    }
                    else 
                    {
                        // result = 0;
                        // Assy子存在しない場合エラー
                        this.ShowMessage("T0100010029");
                        return;
                    }
                    this.shtMeisai[(int)SHEET_COL.UNIT_PRICE, row].Value = Math.Ceiling(result);
                }
                else
                {
                    // 選択行はAssy以外のため、単価計算できません。
                    this.ShowMessage("T0100010027");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }


        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/10/23</create>
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
        /// 全選択クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            this.shtMeisai.Redraw = false;

            this.SetAllCheck(true);

            this.shtMeisai.Redraw = true;
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
            this.shtMeisai.Redraw = false;

            this.SetAllCheck(false);

            this.shtMeisai.Redraw = true;
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
            this.shtMeisai.Redraw = false;

            this.SetRangeCheck(true);

            this.shtMeisai.Redraw = true;
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
            this.shtMeisai.Redraw = false;

            this.SetRangeCheck(false);

            this.shtMeisai.Redraw = true;
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
            this.shtMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtMeisai.MaxRows)
            {
                this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
            }
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 開始ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/02</create>
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

        #endregion

        #region 明細シート
        /// --------------------------------------------------
        /// <summary>
        /// データソース行追加/削除時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/04</create>
        /// <update>N.Ikari 2022/05/10　出荷数が0の時に該当行をグレーアウトする対応</update>
        /// <update>J.Chen 2022/05/18 STEP14</update>
        /// <update>H.Iimuro 2022/10/19 STEP15</update>
        /// <update>H.Iimuro 2022/10/25 処理中の描画停止</update>
        /// <update>Y.Gwon 2023/07/31 返却品の区分が「返却対象」の場合、該当行を紫色にする処理</update>
        /// <update>J.Chen 2023/09/28 返却品の区分「保留」「通関確認中」は「返却対象」と同じ処理とする</update>
        /// --------------------------------------------------
        private void shtMeisai_RowFilling(object sender, RowFillingEventArgs e)
        {
            Sheet sheet = sender as Sheet;
            try
            {
                if (!isRowChange) 
                {
                    debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");

                    int row = e.DestRow;
                    object[] dr = e.SourceRow as object[];

                    //出荷数取得
                    var dt = sheet.DataSource as DataTable;
                    var shukkaQty = sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value == null ? null : sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value.ToString();

                    sheet.Redraw = false;

                    if (sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Value != null)
                    {
                        //出荷数0の時、濃いグレーで表示します。
                        if ("0".Equals(shukkaQty) && (sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Value.ToString() == HENKYAKUHIN_FLAG.DEFAULT_VALUE1))
                        {
                            var color = ComDefine.GRY_COLOR;
                            SetupRowColor(sheet.Rows[row], color, sheet.GridLine, Borders.All);
                        }
                        else if (e.DestRow != -1 && e.OperationMode == OperationMode.Add
                            && this.shtMeisai[ESTIMATE_COLOR, row].Value != null
                            && this.shtMeisai[(int)SHEET_COL.ESTIMATE_FLAG, row].Value.ToString() != ComDefine.NEUTRAL_FLAG)
                        {
                            var offset = dt.Columns[ComDefine.FLD_ESTIMATE_COLOR].Ordinal;
                            var col = ((object[])e.SourceRow)[offset] as string;

                            SetupRowColor(sheet.Rows[row], col, sheet.GridLine, Borders.All);
                        }
                        else if (this.shtMeisai[ESTIMATE_COLOR, row].Value != null
                                    && this.shtMeisai[(int)SHEET_COL.ESTIMATE_FLAG, row].Value.ToString() != ComDefine.NEUTRAL_FLAG)
                        {
                            var col = this.shtMeisai[ESTIMATE_COLOR, row].Value.ToString();
                            SetupRowColor(sheet.Rows[row], col, sheet.GridLine, Borders.All);
                        }
                        else if (sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Value.ToString() != HENKYAKUHIN_FLAG.DEFAULT_VALUE1)
                        {
                            //返却品の区分が「返却対象」の場合、紫色で表示します。
                            var color = ComDefine.PURPLE_COLOR;
                            SetupRowColor(sheet.Rows[row], color, sheet.GridLine, Borders.All);
                        }
                        else
                        {
                            //背景色を元の色に戻す
                            sheet.Rows[row].BackColor = Color.Empty;
                            sheet.Rows[row].DisabledBackColor = Color.FromArgb(223, 223, 223);
                        }
                    }

                    if (e.OperationMode == OperationMode.Add)
                    {
                        // 行追加
                        string tehaiFlg = TEHAI_FLAG.ORDERED_VALUE1;
                        if (dr[(int)SHEET_COL.TEHAI_FLAG] != null && !string.IsNullOrEmpty(dr[(int)SHEET_COL.TEHAI_FLAG].ToString()))
                            tehaiFlg = dr[(int)SHEET_COL.TEHAI_FLAG].ToString();

                        // 手配区分の入力補助(Assy no.採番しない)
                        this.ChangeTehaiFlag(sheet, row, tehaiFlg, false);
                    }
               }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally 
            {
                sheet.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データバインドエラーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_BindingError(object sender, BindingErrorEventArgs e)
        {
            try
            {
                debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");

                Sheet sheet = sender as Sheet;
                int col = e.Position.Column;
                int row = e.Position.Row;

                if (this.IsSheetBindingInvalidate(e))
                {
                    switch (col)
                    {
                        case (int)SHEET_COL.NOUHIN_SAKI:
                        case (int)SHEET_COL.SYUKKA_SAKI:
                            // コンボボックスに項目が存在しない場合、リストに追加する
                            SuperiorComboEditor cbo = sheet[col, row].Editor as SuperiorComboEditor;
                            cbo = AddItemSuperiorComboEditor(cbo, e.Value.ToString());
                            sheet[col, row].Value = e.Value;

                            e.Ignore = true;
                            break;
                        default:
                            break;
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
        /// 行挿入後イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_RowInserted(object sender, RowInsertedEventArgs e)
        {
            debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");
            // 新規行の初期値設定
            Sheet sheet = sender as Sheet;
            // 再描画が抑止されているときはこのイベント処理を継続しない(他のイベントでコールされている) 
            if (sheet == null || !shtMeisai.Redraw)
                return;
            try
            {
                // 新規行のオフセットを計算する
                int row = e.Span.Start - 1;
                
                if (row < 0)
                    return;
                sheet.Redraw = false;
                SetSheetRowInitData(sheet, row);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                sheet.Redraw = true;
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// シートへ初期値を設定
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="row">行番号</param>
        /// <create>D.Okumura 2018/12/18</create>
        /// <update>H.Tsuji 2019/08/27 ユーザーの入力内容に関わらず必ず初期化する不具合</update>
        /// <update>T.Nukaga 2019/11/18 STEP12 返却品管理対応</update>
        /// --------------------------------------------------
        private void SetSheetRowInitData(Sheet sheet, int row)
        {
            // 数量単位
            string quantityUnit = sheet[(int)SHEET_COL.QUANTITY_UNIT, row].Value == null ? null : sheet[(int)SHEET_COL.QUANTITY_UNIT, row].Value.ToString();
            if (string.IsNullOrEmpty(quantityUnit))
            {
                sheet[(int)SHEET_COL.QUANTITY_UNIT, row].Value = QUANTITY_UNIT.DEFAULT_VALUE1;
            }
            // 手配区分
            string tehaiFlag = sheet[(int)SHEET_COL.TEHAI_FLAG, row].Value == null ? null : sheet[(int)SHEET_COL.TEHAI_FLAG, row].Value.ToString();
            if (string.IsNullOrEmpty(tehaiFlag))
            {
                sheet[(int)SHEET_COL.TEHAI_FLAG, row].Value = TEHAI_FLAG.DEFAULT_VALUE1;
            }
            // 返却品フラグ
            string henkyakuhinFlag = sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Value == null ? null : sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Value.ToString();
            if (string.IsNullOrEmpty(henkyakuhinFlag))
            {
                sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Value = HENKYAKUHIN_FLAG.DEFAULT_VALUE1;
            }
        }

        #region セル入力内容変更時関連

        #region セル入力内容変更時

        /// --------------------------------------------------
        /// <summary>
        /// セル入力内容変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/03</create>
        /// <update>K.Tsutsumi 2019/03/08 型式なしで登録されているパーツの場合に、画面上で型式を消さないと検索がヒットしない不具合を修正</update>
        /// <update>H.Tsuji 2019/08/25 STEP10 組立パーツの識別処理を追加</update>
        /// <update>T.Nukaga 2019/12/16 STEP12 返却品が対象の場合、出荷数は0</update>
        /// <update>J.Chen 2023/10/13 明細分割時、返却フラグ変更による出荷数の修正補助を行わないようにする</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// --------------------------------------------------
        private void shtMeisai_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");
                
                Sheet sheet = sender as Sheet;
                int row = e.Position.Row;
                string hinmei_jp = sheet[(int)SHEET_COL.HINMEI_JP, row].Value == null ? null : sheet[(int)SHEET_COL.HINMEI_JP, row].Value.ToString();
                string zumen = sheet[(int)SHEET_COL.ZUMEN_KEISHIKI, row].Value == null ? null : sheet[(int)SHEET_COL.ZUMEN_KEISHIKI, row].Value.ToString();
                string zumen2 = sheet[(int)SHEET_COL.ZUMEN_KEISHIKI2, row].Value == null ? null : sheet[(int)SHEET_COL.ZUMEN_KEISHIKI2, row].Value.ToString();
                string tehaiFlg = sheet[(int)SHEET_COL.TEHAI_FLAG, row].Value == null ? null : sheet[(int)SHEET_COL.TEHAI_FLAG, row].Value.ToString();
                string hinmei = sheet[(int)SHEET_COL.HINMEI, row].Value == null ? null : sheet[(int)SHEET_COL.HINMEI, row].Value.ToString();

                switch (e.Position.Column)
                {
                    // イベント発火しないのでタイミング変更
                    //case (int)SHEET_COL.NOUHIN_SAKI:
                    //case (int)SHEET_COL.SYUKKA_SAKI:
                    //    // コンボボックスに項目が存在しない場合、リストに追加する
                    //    int col = e.Position.Column;
                    //    if (sheet[col, row].Value == null)
                    //    {
                    //        SuperiorComboEditor cbo = sheet[col, row].Editor as SuperiorComboEditor;
                    //        cbo = AddItemSuperiorComboEditor(cbo, sheet[col, row].Text);
                    //        sheet[col, row].Value = sheet[col, row].Text;
                    //    }
                    //    break;
                    case (int)SHEET_COL.HINMEI_JP:
                        // 品名(和文)時の入力補助
                        // 品番/型式がIS NULLで絞り込む。左から順に入力されるとき補完として機能しないが、
                        // 現行動作と合わせるため、この処理でよい。(ただし、図番/型式が空欄となるため保存時に入力チェックに引っかかる)
                        // → K.Tsutsumi 2019/03/08
                        //    品名が設定されていないときにデータベースへ参照するように修正
                        //    型式を消さないと検索ができない部分を修正
                        //    データ取得できた際に NULL を型式へ上書きしている部分を修正
                        //    データが取得できなかったときは、型式以外クリアするように修正
                        if (!string.IsNullOrEmpty(hinmei_jp) && string.IsNullOrEmpty(hinmei))
                        {
                            ConnT01 conn = new ConnT01();
                            DataTable dt = conn.GetPartsName(new CondT01(this.UserInfo)
                            {
                                HinmeiJp = hinmei_jp
                                , PartsCode = null

                            }).Tables[Def_M_PARTS_NAME.Name];

                            if (dt != null && dt.Rows.Count == 1)
                            {
                                sheet.Redraw = false;
                                sheet[(int)SHEET_COL.HINMEI, row].Value = ComFunc.GetFld(dt, 0, Def_M_PARTS_NAME.HINMEI, string.Empty);
                                sheet[(int)SHEET_COL.HINMEI_INV, row].Value = ComFunc.GetFld(dt, 0, Def_M_PARTS_NAME.HINMEI_INV, string.Empty);
                                sheet[(int)SHEET_COL.FREE2, row].Value = ComFunc.GetFld(dt, 0, Def_M_PARTS_NAME.FREE2, string.Empty);
                                sheet[(int)SHEET_COL.MAKER, row].Value = ComFunc.GetFld(dt, 0, Def_M_PARTS_NAME.MAKER, string.Empty);
                                sheet[(int)SHEET_COL.NOTE, row].Value = ComFunc.GetFld(dt, 0, Def_M_PARTS_NAME.NOTE, string.Empty);
                                sheet[(int)SHEET_COL.CUSTOMS_STATUS, row].Value = ComFunc.GetFld(dt, 0, Def_M_PARTS_NAME.CUSTOMS_STATUS, string.Empty);
                                sheet.Redraw = true;
                            }
                            else
                            { 
                                // マスタが取得できなかった場合は、型式以外をクリアする
                                sheet.Redraw = false;
                                sheet[(int)SHEET_COL.HINMEI, row].Value = string.Empty;
                                sheet[(int)SHEET_COL.HINMEI_INV, row].Value = string.Empty;
                                sheet[(int)SHEET_COL.FREE2, row].Value = string.Empty;
                                sheet[(int)SHEET_COL.MAKER, row].Value = string.Empty;
                                sheet[(int)SHEET_COL.NOTE, row].Value = string.Empty;
                                sheet[(int)SHEET_COL.CUSTOMS_STATUS, row].Value = string.Empty;
                                sheet.Redraw = true;
                            }
                        }
                        break;
                    case (int)SHEET_COL.ZUMEN_KEISHIKI:
                        {
                            // 図番/型式の入力補助
                            this.AssistInputZumenKeishiki(sheet, row);
                            break;
                        }
                    case (int)SHEET_COL.ZUMEN_KEISHIKI2:
                        {
                            // 図番の入力補助
                            this.AssistInputZumenKeishiki2(sheet, row);
                            break;
                        }
                    case (int)SHEET_COL.TEHAI_QTY:
                        // 手配数時の入力補助
                        if (sheet.ActiveCell.Value != null && TEHAI_FLAG.ORDERED_VALUE1.Equals(tehaiFlg))
                        {
                            decimal tehaiQty;
                            if (decimal.TryParse(sheet[(int)SHEET_COL.TEHAI_QTY, row].Value.ToString(), out tehaiQty))
                            {
                                decimal hacchuQty = DSWUtil.UtilConvert.ToDecimal(sheet[(int)SHEET_COL.HACCHU_QTY, row].Value, 0m);
                                if (hacchuQty == 0m)
                                    sheet[(int)SHEET_COL.HACCHU_QTY, row].Value = tehaiQty;

                                decimal shukkaQty = DSWUtil.UtilConvert.ToDecimal(sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value, 0m);
                                if (shukkaQty == 0m)
                                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value = tehaiQty;

                                // 返却品フラグ対象時は出荷数入力不可
                                this.ChangeHenkyakuhinFlag(sheet, row, true);
                            }
                        }
                        // SKS_Skip場合
                        if (sheet.ActiveCell.Value != null && TEHAI_FLAG.SKS_SKIP_VALUE1.Equals(tehaiFlg))
                        {
                            decimal tehaiQty;
                            if (decimal.TryParse(sheet[(int)SHEET_COL.TEHAI_QTY, row].Value.ToString(), out tehaiQty))
                            {
                                decimal shukkaQty = DSWUtil.UtilConvert.ToDecimal(sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value, 0m);
                                if (shukkaQty == 0m)
                                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value = tehaiQty;
                            }
                        }
                        break;
                    case (int)SHEET_COL.TEHAI_FLAG:
                        // 手配区分時の入力補助(Assy no.採番)
                        this.ChangeTehaiFlag(sheet, row, tehaiFlg, true);
                        break;
                    case (int)SHEET_COL.ASSY_NO:
                        // Assy no.の入力補助
                        this.AssistInputAssyNo(sheet, row);
                        break;
                    case (int)SHEET_COL.HENKYAKUHIN_FLAG:
                        // 返却品フラグ対象時は出荷数入力不可
                        isHenkyakuhinFlagChange = true;
                        this.ChangeHenkyakuhinFlag(sheet, row, true);
                        isHenkyakuhinFlagChange = false;
                        break;
                    default:
                        break;
                }

                return;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力補助(図番/型式)

        /// --------------------------------------------------
        /// <summary>
        /// 入力補助(図番/型式)
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="row">行インデックス</param>
        /// <create>H.Tsuji 2019/08/25</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// <remarks>
        /// <p>
        /// [K.Tsutsumi 2019/03/08]
        /// ・品名が設定されていないときにデータベースへ参照するように修正
        /// ・データが取得できなかったときは、クリアするように修正
        /// [H.Tsuji 2019/08/25]
        /// ・写真の登録チェック処理を追加
        /// ・品名が設定されていないときは写真の存在確認と同タイミングでパーツ名翻訳マスタを検索する
        /// </p>
        /// </remarks>
        /// --------------------------------------------------
        private void AssistInputZumenKeishiki(Sheet sheet, int row)
        {
            string zumen = sheet[(int)SHEET_COL.ZUMEN_KEISHIKI, row].Value == null ? null : sheet[(int)SHEET_COL.ZUMEN_KEISHIKI, row].Value.ToString();
            string zumen2 = sheet[(int)SHEET_COL.ZUMEN_KEISHIKI2, row].Value == null ? null : sheet[(int)SHEET_COL.ZUMEN_KEISHIKI2, row].Value.ToString();
            string hinmei = sheet[(int)SHEET_COL.HINMEI, row].Value == null ? null : sheet[(int)SHEET_COL.HINMEI, row].Value.ToString();

            ConnT01 conn = new ConnT01();
            CondT01 cond = new CondT01(this.UserInfo)
            {
                ZumenKeishiki = zumen,
                ZumenKeishiki2 = zumen2,
                PartsCode = zumen,
                Hinmei = hinmei,
            };

            // 図番/型式時の入力補助のためデータ検索
            DataSet ds = conn.GetZumenKeishikiInputAssistance(cond);
            DataTable dtManageZumenKeishiki = ds.Tables[Def_T_MANAGE_ZUMEN_KEISHIKI.Name];

            // 以降、検索データをシートに反映
            try
            {
                // 再描画を一時停止
                sheet.Redraw = false;

                // 写真登録状況をシートに反映する
                sheet[(int)SHEET_COL.PHOTO, row].Value = (dtManageZumenKeishiki != null && dtManageZumenKeishiki.Rows.Count != 0) ? ComDefine.EXISTS_PICTURE_VALUE : string.Empty;

                // パーツ関連のデータをシートに反映する
                if (!string.IsNullOrEmpty(zumen) && string.IsNullOrEmpty(hinmei))
                {
                    DataTable dtPartsName = ds.Tables.Contains(Def_M_PARTS_NAME.Name) ? ds.Tables[Def_M_PARTS_NAME.Name] : null;
                    if (dtPartsName != null && dtPartsName.Rows.Count == 1)
                    {
                        // 品名から設定しないと品名(和文)のValueChangedイベントで２回検索する
                        sheet[(int)SHEET_COL.HINMEI, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.HINMEI, string.Empty);
                        sheet[(int)SHEET_COL.HINMEI_JP, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.HINMEI_JP, string.Empty);
                        sheet[(int)SHEET_COL.HINMEI_INV, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.HINMEI_INV, string.Empty);
                        sheet[(int)SHEET_COL.FREE2, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.FREE2, string.Empty);
                        sheet[(int)SHEET_COL.MAKER, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.MAKER, string.Empty);
                        sheet[(int)SHEET_COL.NOTE, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.NOTE, string.Empty);
                        sheet[(int)SHEET_COL.CUSTOMS_STATUS, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.CUSTOMS_STATUS, string.Empty);
                    }
                    else
                    {
                        sheet[(int)SHEET_COL.HINMEI_JP, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.HINMEI, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.HINMEI_INV, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.FREE2, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.MAKER, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.NOTE, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.CUSTOMS_STATUS, row].Value = string.Empty;
                    }
                }
            }
            finally
            {
                // 必ず再描画をかける
                sheet.Redraw = true;
            }
        }

        #endregion

        #region 入力補助(図番)

        /// --------------------------------------------------
        /// <summary>
        /// 入力補助(図番)
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="row">行インデックス</param>
        /// <create>H.Tsuji 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void AssistInputZumenKeishiki2(Sheet sheet, int row)
        {
            string zumen = sheet[(int)SHEET_COL.ZUMEN_KEISHIKI, row].Value == null ? null : sheet[(int)SHEET_COL.ZUMEN_KEISHIKI, row].Value.ToString();
            string zumen2 = sheet[(int)SHEET_COL.ZUMEN_KEISHIKI2, row].Value == null ? null : sheet[(int)SHEET_COL.ZUMEN_KEISHIKI2, row].Value.ToString();

            ConnT01 conn = new ConnT01();
            CondT01 cond = new CondT01(this.UserInfo)
            {
                ZumenKeishiki = zumen,
                ZumenKeishiki2 = zumen2,
            };

            // 図番/型式時の入力補助のためデータ検索
            DataSet ds = conn.GetManageZumenKeishikiInSearch(cond);
            DataTable dt = ds.Tables[Def_T_MANAGE_ZUMEN_KEISHIKI.Name];

            // 以降、検索データをシートに反映
            try
            {
                // 再描画を一時停止
                sheet.Redraw = false;

                // 写真登録状況をシートに反映する
                sheet[(int)SHEET_COL.PHOTO, row].Value = (dt != null && dt.Rows.Count != 0) ? ComDefine.EXISTS_PICTURE_VALUE : string.Empty;
            }
            finally
            {
                // 必ず再描画をかける
                sheet.Redraw = true;
            }
        }

        #endregion

        #region 入力補助(Assy no.)

        /// --------------------------------------------------
        /// <summary>
        /// 入力補助(Assy no.)
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="row">行インデックス</param>
        /// <create>H.Tsuji 2019/08/26</create>
        /// <update>T.Nukaga 2019/12/16 STEP12 返却品対応</update>
        /// <update>J.Jeong 2024/07/10 出荷数の制御</update>
        /// --------------------------------------------------
        private void AssistInputAssyNo(Sheet sheet, int row)
        {
            string assyNo = sheet[(int)SHEET_COL.ASSY_NO, row].Value == null ? null : sheet[(int)SHEET_COL.ASSY_NO, row].Value.ToString();
            string tehaiFlg = sheet[(int)SHEET_COL.TEHAI_FLAG, row].Value == null ? null : sheet[(int)SHEET_COL.TEHAI_FLAG, row].Value.ToString();

            // 手配区分「Assy」の場合はここで終わり
            if (TEHAI_FLAG.ASSY_VALUE1.Equals(tehaiFlg)) return;

            // 以降、検索データをシートに反映
            try
            {
                // 再描画を一時停止
                sheet.Redraw = false;

                // 出荷数の制御
                if (string.IsNullOrEmpty(assyNo) && !TEHAI_FLAG.CANCELLED_VALUE1.Equals(tehaiFlg))
                {
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Enabled = true;

                    // 返却品対象の場合は出荷数入力不可
                    this.ChangeHenkyakuhinFlag(sheet, row, false);
                }
                else
                {
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value = null;
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Enabled = false;
                }
            }
            finally
            {
                // 必ず再描画をかける
                sheet.Redraw = true;
            }
        }

        #endregion

        #region 入力補助(図番/型式)

        /// --------------------------------------------------
        /// <summary>
        /// Excel取込用入力補助(図番/型式)
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="row">行インデックス</param>
        /// <create>J.Chen 2022/11/01</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void AssistInputZumenKeishikiImp(Sheet sheet, int row, string hinmei_jp, string hinmei)
        {
            string zumen = sheet[(int)SHEET_COL.ZUMEN_KEISHIKI, row].Value == null ? null : sheet[(int)SHEET_COL.ZUMEN_KEISHIKI, row].Value.ToString();

            ConnT01 conn = new ConnT01();
            CondT01 cond = new CondT01(this.UserInfo)
            {
                ZumenKeishiki = zumen,
                PartsCode = zumen,
            };

            // 図番/型式時の入力補助のためデータ検索
            DataSet ds = conn.GetZumenKeishikiInputAssistance(cond);
            DataTable dtManageZumenKeishiki = ds.Tables[Def_T_MANAGE_ZUMEN_KEISHIKI.Name];

            // 以降、検索データをシートに反映
            try
            {
                // 再描画を一時停止
                sheet.Redraw = false;

                // 写真登録状況をシートに反映する
                sheet[(int)SHEET_COL.PHOTO, row].Value = (dtManageZumenKeishiki != null && dtManageZumenKeishiki.Rows.Count != 0) ? ComDefine.EXISTS_PICTURE_VALUE : string.Empty;

                // パーツ関連のデータをシートに反映する
                if (!string.IsNullOrEmpty(zumen))
                {
                    DataTable dtPartsName = ds.Tables.Contains(Def_M_PARTS_NAME.Name) ? ds.Tables[Def_M_PARTS_NAME.Name] : null;
                    if (dtPartsName != null && dtPartsName.Rows.Count == 1)
                    {
                        // 品名、品名（和文）が既に存在した場合、補助機能使用しない
                        sheet[(int)SHEET_COL.HINMEI, row].Value = string.IsNullOrEmpty(hinmei) ? ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.HINMEI, string.Empty) : hinmei;
                        sheet[(int)SHEET_COL.HINMEI_JP, row].Value = string.IsNullOrEmpty(hinmei_jp) ? ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.HINMEI_JP, string.Empty) : hinmei_jp;

                        sheet[(int)SHEET_COL.HINMEI_INV, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.HINMEI_INV, string.Empty);
                        sheet[(int)SHEET_COL.FREE2, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.FREE2, string.Empty);
                        sheet[(int)SHEET_COL.MAKER, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.MAKER, string.Empty);
                        sheet[(int)SHEET_COL.NOTE, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.NOTE, string.Empty);
                        sheet[(int)SHEET_COL.CUSTOMS_STATUS, row].Value = ComFunc.GetFld(dtPartsName, 0, Def_M_PARTS_NAME.CUSTOMS_STATUS, string.Empty);
                    }
                    else
                    {
                        sheet[(int)SHEET_COL.HINMEI_JP, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.HINMEI, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.HINMEI_INV, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.FREE2, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.MAKER, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.NOTE, row].Value = string.Empty;
                        sheet[(int)SHEET_COL.CUSTOMS_STATUS, row].Value = string.Empty;
                    }
                }
            }
            finally
            {
                // 必ず再描画をかける
                sheet.Redraw = true;
            }
        }

        #endregion

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// セル入力時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/11/30</create>
        /// <update>D.Okumura 2019/01/25 文字入力制限を文字数からバイト数へ変更</update>
        /// <update>J.Chen 2024/08/19 出荷先桁数変更</update>
        /// --------------------------------------------------
        private void shtMeisai_CellNotify(object sender, CellNotifyEventArgs e)
        {
            try
            {
                debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call:" + e.Name);
                
                Sheet sheet = sender as Sheet;
                int row = e.Position.Row;

                switch (e.Name)
                {
                    case CellNotifyEvents.SelectedIndexChanged:
                    case CellNotifyEvents.TextChanged:
                        switch (e.Position.Column)
                        {
                            case (int)SHEET_COL.NOUHIN_SAKI:
                                // 文字数制限
                                if (sheet.ActiveCell.Value != null)
                                    break;
                                if (UtilString.GetByteCount(sheet.ActiveCell.Text) > 10)
                                    sheet.ActiveCell.Text = UtilString.SubstringForByte(sheet.ActiveCell.Text, 0, 10);
                                selectStart(sheet.ActiveCell.Editor, sheet.ActiveCell.Text.Length);
                                break;
                            case (int)SHEET_COL.SYUKKA_SAKI:
                                // 文字数制限
                                if (sheet.ActiveCell.Value != null)
                                    break;
                                if (UtilString.GetByteCount(sheet.ActiveCell.Text) > 20)
                                    sheet.ActiveCell.Text = UtilString.SubstringForByte(sheet.ActiveCell.Text, 0, 20);
                                selectStart(sheet.ActiveCell.Editor, sheet.ActiveCell.Text.Length);
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// セルフォーカスアウト時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/07</create>
        /// <update>>N.Ikari 2022/05/25 出荷先存在しない場合エラー</update>
        /// <update>J.Chen 2022/11/25 手配Noチェック</update>
        /// --------------------------------------------------
        private void shtMeisai_LeaveCell(object sender, LeaveCellEventArgs e)
        {
            try{
                debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");
                
                Sheet sheet = sender as Sheet;
                AddNoneItemSuperiorComboEditor(sheet, sheet.ActivePosition.Row);

                if (sheet[(int)SHEET_COL.SYUKKA_SAKI, sheet.ActivePosition.Row].Text != "")
                {
                    SuperiorComboEditor cbo = sheet[(int)SHEET_COL.SYUKKA_SAKI, sheet.ActivePosition.Row].Editor as SuperiorComboEditor;
                    if (cbo.FindStringExact(sheet[(int)SHEET_COL.SYUKKA_SAKI, sheet.ActivePosition.Row].Text, TargetMember.DisplayMember) < 1)
                    {
                        sheet[(int)SHEET_COL.SYUKKA_SAKI, sheet.ActivePosition.Row].Text = null;
                        this.ShowMessage("T0100010028");
                    }
                    else
                    {
                        if (sheet.ActivePosition.Column == (int)SHEET_COL.SYUKKA_SAKI)
                        {
                            this.ClearMessage();
                        }
                        sheet[(int)SHEET_COL.SYUKKA_SAKI, sheet.ActivePosition.Row].Value = sheet[(int)SHEET_COL.SYUKKA_SAKI, sheet.ActivePosition.Row].Text;

                    }
                }

                // 手配Noを変更前の状態に戻す
                Action<string> actUndo = tehaiNo =>
                {
                    if (this.shtMeisai.ActiveCell.Tag == null || string.IsNullOrEmpty(this.shtMeisai.ActiveCell.Tag.ToString()))
                    {
                        this.shtMeisai.ActiveCell.Text = tehaiNo;
                        return;
                    }
                    this.shtMeisai.ActiveCell.Text = this.shtMeisai.Tag.ToString();
                };

                if (sheet.ActivePosition.Column == (int)SHEET_COL.TEHAI_NO)
                {
                    this.ClearMessage();
                    var tehaiNo = this.shtMeisai.ActiveCell.Text;

                    // 手配No検索
                    var conn = new ConnT01();
                    var cond = new CondT01(this.UserInfo);

                    // 重複排除
                    string[] array = tehaiNo.Split('+').Distinct().ToArray();
                    string tehaiNoAll = "";
                    string errMsgID = null;
                    string[] args = null;
                    DataSet ds = null;

                    if (dtTemp != null)
                    {
                        DataRow[] dr_count = dtTemp.Select(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO + " = '" + sheet[(int)SHEET_COL.TEHAI_RENKEI_NO, sheet.ActivePosition.Row].Text + "'");
                        if (dr_count.Count() > 0)
                        {
                            string tehaiNoTemp = dr_count[0][Def_T_TEHAI_SKS.TEHAI_NO].ToString();
                            string[] array1 = tehaiNoTemp.Split('+').Distinct().ToArray();
                            string[] array2 = array1.Except(array).ToArray();
                            int shukkaMeisaiCnt = 0;
                            int.TryParse(sheet[(int)SHEET_COL.SHUKKA_MEISAI_CNT, sheet.ActivePosition.Row].Text, out shukkaMeisaiCnt);

                            if (array2.Length > 0 && shukkaMeisaiCnt > 0)
                            {
                                this.ShowMessage("T0100020016");
                                this.shtMeisai.ActiveCell.Text = tehaiNoTemp;
                                return;
                            }
                        }
                    }

                    for (int i = 0; i < array.Count(); i++)
                    {
                        // 8桁数未満
                        if (array[i].Length != 8)
                        {
                            array[i] = null;

                        }
                        else
                        {
                            if ((i < array.Count()) && !string.IsNullOrEmpty(array[i]))
                            {
                                cond.TehaiNo = array[i];
                                cond.TehaiKindFlag = sheet[(int)SHEET_COL.TEHAI_KIND_FLAG, sheet.ActivePosition.Row].Text;
                                cond.TehaiRenkeiNo = sheet[(int)SHEET_COL.TEHAI_RENKEI_NO, sheet.ActivePosition.Row].Text;
                                cond.EcsNo = this.txtEcsNo.Text;

                                ds = conn.GetSKSTehaiRenkeiTehaiSKSWork(cond, out errMsgID, out args);

                            }

                            // 連携済みチェック
                            if (!string.IsNullOrEmpty(errMsgID))
                            {
                                this.ShowMessage(errMsgID, args);
                                array[i] = null;
                            }

                            // 存在チェック
                            //if (!UtilData.ExistsData(ds, Def_T_TEHAI_SKS_WORK.Name))
                            //{
                            //    //// 該当手配Noは存在しません。
                            //    //this.ShowMessage("T0100020021");
                            //    array[i] = null;
                            //}
                        }

                        if (!string.IsNullOrEmpty(array[i]))
                        {
                            tehaiNoAll += array[i] + delimiter;
                        }
                    }

                    if (tehaiNoAll.Length < tehaiNo.Length)
                    {
                        actUndo(tehaiNoAll);
                        // 使用できない手配Noが存在しています。
                        this.ShowMessage("T0100010035");
                    }

                    // 最後の文字を削除
                    if (sheet.ActiveCell.Text.EndsWith(delimiter))
                    {
                        sheet.ActiveCell.Text = sheet.ActiveCell.Text.Remove(tehaiNoAll.Length - 1);
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
        /// 行更新時時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/22</create>
        /// <update>R.Sumi 2022/03/17 出荷数が0の時に該当行をグレーアウトする対応</update>
        /// <update>N.Ikari 2022/05/10 上記対応をデータソース行追加/削除時処理へ移動</update>
        /// --------------------------------------------------
        private void shtMeisai_RowUpdating(object sender, RowUpdatingEventArgs e)
        {
            try
            {
                debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");

                Sheet sheet = sender as Sheet;
                int row = e.DestRow;
                object[] dr = e.SourceRow as object[];


                if (e.OperationMode == OperationMode.Change)
                {
                    // 納品先
                    if (sheet[(int)SHEET_COL.NOUHIN_SAKI, row].Value == null)
                    {
                        AddNoneItemSuperiorComboEditor(sheet, row);
                        // DataSourceとグリッド表示が異なる場合の対策
                        if (dr[(int)SHEET_COL.NOUHIN_SAKI] != sheet[(int)SHEET_COL.NOUHIN_SAKI, row].Value)
                            dr[(int)SHEET_COL.NOUHIN_SAKI] = sheet[(int)SHEET_COL.NOUHIN_SAKI, row].Value;

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
        /// 行の背景色、及び前景色を変更する
        /// </summary>
        /// <param name="row">列</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>R.Sumi 2022/03/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetupRowColor(Row row, string input, GridLine baseGridLine, Borders border)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var cols = input.Split(',');
            if (cols.Length < 2)
            {
                return;
            }

            var backcolor = ComFunc.GetColorFromRgb(cols[1]);
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

            var forecolor = ComFunc.GetColorFromRgb(cols[0]);
            if (forecolor != null)
            {
                row.ForeColor = forecolor ?? row.ForeColor;
                row.DisabledForeColor = forecolor ?? row.ForeColor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 行の背景色、及び前景色を初期の色にする
        /// </summary>
        /// <param name="row">列</param>
        /// <create>H.Iimuro 2022/10/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void defaultRowBackColor(Row row)
        {
            row.BackColor = Color.Empty;
            row.ForeColor = Color.Empty;
            row.DisabledBackColor = Color.FromArgb(223, 223, 223); 
            row.DisabledForeColor = Color.Black;
        }

        
        /// --------------------------------------------------
        /// <summary>
        /// 編集終了時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2019/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_LeaveEdit(object sender, LeaveEditEventArgs e)
        {
            try
            {
                debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");

                Sheet sheet = sender as Sheet;

                // Assy no.をゼロパディングする
                if (sheet.ActivePosition.Column == (int)SHEET_COL.ASSY_NO && !string.IsNullOrEmpty(sheet.ActiveCell.Text))
                {
                    sheet.ActiveCell.Text = UtilString.PadLeft(sheet.ActiveCell.Text, 5, '0');
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートフォーカスアウト時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_Leave(object sender, EventArgs e)
        {
            try
            {
                debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");

                if (rdoAllDelete.Checked) return; // 全削除の場合、なにもしない

                int rowCnt = this.shtMeisai.Rows.Count - 1; // 新規行を除く
                for (int i = 0; i < rowCnt; i++)
                    AddNoneItemSuperiorComboEditor(this.shtMeisai, i);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// テキストの開始点設定
        /// </summary>
        /// <param name="editor">エディタ</param>
        /// <param name="start">開始位置</param>
        /// <create>D.Naito 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private void selectStart(GridEditor editor, int start)
        {
            try
            {
                if (editor is SuperiorComboEditor)
                {
                    var obj = editor as SuperiorComboEditor;
                    obj.SelectionStart = start;
                }
                else if (editor is TextEditor)
                {
                    var obj = editor as TextEditor;
                    obj.SelectionStart = start;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 手配区分変更時(入力補助)
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="row">行数</param>
        /// <param name="tehaiFlg">手配区分</param>
        /// <param name="isAssySaibanOK">Assy採番許可 true:許可, false:許可しない</param>
        /// <create>D.Naito 2018/12/06</create>
        /// <update>H.Tsuji 2019/08/25 STEP10 組立パーツの識別処理を追加</update>
        /// <update>H.Tsuji 2019/09/09 手配情報検索時に手配区分「Assy」の行数分Assy no.を採番してしまう不具合修正</update>
        /// <update>T.Nukaga 2019/12/16 STEP12 返却品対応</update>
        /// <update>J.Chen 2022/06/03 STEP14 返品登録制御</update>
        /// <update>J.Chen 2022/11/11 手配No登録制御</update>
        /// <update>J.Jeong 2024/07/10 出荷数の制御</update>
        /// <remarks>
        /// <para>
        /// RowFillingイベントでもAssy no.が採番されてしまうため、Assy採番許可フラグを引数に追加。
        /// 手配区分のValueChangedイベント発火時のみ、Assy no.を採番することを想定。
        /// </para>
        /// </remarks>
        /// --------------------------------------------------
        private void ChangeTehaiFlag(Sheet sheet, int row, string tehaiFlg, bool isAssySaibanOK)
        {
            // 手配区分の入力補助
            try
            {
                string assyNo = sheet[(int)SHEET_COL.ASSY_NO, row].Value == null ? null : sheet[(int)SHEET_COL.ASSY_NO, row].Value.ToString();

                // 手配区分「発注」
                if (TEHAI_FLAG.ORDERED_VALUE1.Equals(tehaiFlg))
                {
                    sheet[(int)SHEET_COL.HACCHU_QTY, row].Enabled = true;
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Enabled = true;
                    sheet[(int)SHEET_COL.UNIT_PRICE, row].Value = null;
                    sheet[(int)SHEET_COL.UNIT_PRICE, row].Enabled = false;
                    sheet[(int)SHEET_COL.ASSY_NO, row].Enabled = true;
                    sheet[(int)SHEET_COL.TEHAI_NO, row].Enabled = true;
                }
                else 
                {
                    if (string.IsNullOrEmpty(sheet[(int)SHEET_COL.TEHAI_RENKEI_NO, row].Text))
                    {
                        sheet[(int)SHEET_COL.TEHAI_NO, row].Text = null;
                    }
                    sheet[(int)SHEET_COL.TEHAI_NO, row].Enabled = false;
                }
                // 手配区分「Assy」
                if (TEHAI_FLAG.ASSY_VALUE1.Equals(tehaiFlg))
                {
                    sheet[(int)SHEET_COL.HACCHU_QTY, row].Value = null;
                    sheet[(int)SHEET_COL.HACCHU_QTY, row].Enabled = false;
                    sheet[(int)SHEET_COL.UNIT_PRICE, row].Enabled = true;
                    sheet[(int)SHEET_COL.ASSY_NO, row].Enabled = false;

                    if (isAssySaibanOK)
                    {
                        // 採番時エラーに備えてクリアしておく
                        sheet[(int)SHEET_COL.ASSY_NO, row].Value = null;

                        // Assy no.採番
                        string saibanAssyNo;
                        string errMsgID;
                        if (!this.GetSaibanAssyNo(out saibanAssyNo, out errMsgID))
                        {
                            if (!string.IsNullOrEmpty(errMsgID))
                            {
                                this.ShowMessage(errMsgID);
                            }
                        }
                        sheet[(int)SHEET_COL.ASSY_NO, row].Value = saibanAssyNo;
                    }
                }
                // 手配区分「SKS Skip」「余剰品」
                if (TEHAI_FLAG.SKS_SKIP_VALUE1.Equals(tehaiFlg) || TEHAI_FLAG.SURPLUS_VALUE1.Equals(tehaiFlg))
                {
                    sheet[(int)SHEET_COL.HACCHU_QTY, row].Value = null;
                    sheet[(int)SHEET_COL.HACCHU_QTY, row].Enabled = false;
                    sheet[(int)SHEET_COL.UNIT_PRICE, row].Enabled = true;
                    sheet[(int)SHEET_COL.ASSY_NO, row].Enabled = true;

                }
                // 手配区分「Cancel」
                if (TEHAI_FLAG.CANCELLED_VALUE1.Equals(tehaiFlg))
                {
                    sheet[(int)SHEET_COL.HACCHU_QTY, row].Value = null;
                    sheet[(int)SHEET_COL.HACCHU_QTY, row].Enabled = false;
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value = null;
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Enabled = false;
                    sheet[(int)SHEET_COL.UNIT_PRICE, row].Enabled = true;
                    sheet[(int)SHEET_COL.ASSY_NO, row].Enabled = false;
                }

                // 出荷数の制御
                if (!TEHAI_FLAG.ASSY_VALUE1.Equals(tehaiFlg) && !string.IsNullOrEmpty(assyNo))
                {
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value = null;
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Enabled = false;
                }
                else
                {
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Enabled = true;

                    // 返却品対象の場合は出荷数入力不可
                    this.ChangeHenkyakuhinFlag(sheet, row, false);
                }

                // 返品登録の制御
                if (TEHAI_FLAG.SURPLUS_VALUE1.Equals(tehaiFlg) || TEHAI_FLAG.SKS_SKIP_VALUE1.Equals(tehaiFlg))
                {
                    // 初期値設定
                    sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Value = HENKYAKUHIN_FLAG.DEFAULT_VALUE1;
                    sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Enabled = false;
                }
                else
                {
                    sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Enabled = true;
                }

                // 手配区分「Cancel」(出荷数制限)
                if (TEHAI_FLAG.CANCELLED_VALUE1.Equals(tehaiFlg))
                {
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value = null;
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region OnClosing

        /// --------------------------------------------------
        /// <summary>
        /// OnClosing
        /// </summary>
        /// <param name="e">CancelEventArgs</param>
        /// <create>H.Tsuji 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                base.OnClosing(e);

                // フォームを閉じる前にグリッド幅を設定ファイルに保存する
                if (!e.Cancel)
                {
                    if (!this.SaveGridSetting(this.shtMeisai, this._iniSectionPrefix))
                    {
                        this.ShowMessage("A9999999076");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Enterキー操作のラジオボタン切替

        /// --------------------------------------------------
        /// <summary>
        /// Enterキー操作のラジオボタン切替
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2019/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoKeyAction_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // メッセージクリア
                this.ClearMessage();

                // ショートカットキーを登録する
                this.shtMeisai.ShortCuts.Remove(Keys.Enter);
                
                // ラジオボタンはチェックするとTabStopも自動でtrueとなるため、その都度falseにする必要がある
                if (this.rdoRight.Checked)
                {
                    this.shtMeisai.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.NextCellWithWrap });
                    this.rdoRight.TabStop = false;
                }
                else
                {
                    this.shtMeisai.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.EndEdit, KeyAction.NextRow });
                    this.rdoDown.TabStop = false;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region シートイベント

        
        /// --------------------------------------------------
        /// <summary>
        /// RRGGBB形式の文字列からColorオブジェクトを生成する
        /// </summary>
        /// <param name="input">RGB文字列</param>
        /// <returns>色</returns>
        /// <create>H.Iimuro 2022/10/19</create>
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

        #region コンボボックス
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスセット
        /// </summary>
        /// <create>S.Furugo 2018/11/08</create>
        /// <update>T.Nukaga 2019/11/15 STEP12 返却品管理対応 返却品追加 </update>
        /// <update>Y.Gwon 2023/07/04 STEP16 返却品を出荷制限に変更</update>
        /// --------------------------------------------------
        private void SetComboBox()
        {
            ConnT01 conn = new ConnT01();
            // --- 技連 ---
            // プロジェクト(物件名)一覧を設定
            DataSet dsBukken = conn.GetBukkenName();
            if (ComFunc.IsExistsData(dsBukken, Def_M_PROJECT.Name))
            {
                this.cboBukkenName.DisplayMember = Def_M_PROJECT.BUKKEN_NAME;
                this.cboBukkenName.ValueMember = Def_M_PROJECT.PROJECT_NO;
                this.cboBukkenName.DataSource = dsBukken.Tables[Def_M_PROJECT.Name];
            }

            // 機種
            DataSet dtKishu = conn.GetSelectItem(new CondT01(this.UserInfo) { SelectGroupCode = SELECT_GROUP_CD.KISHU_VALUE1 });
            if (ComFunc.IsExistsData(dtKishu, Def_M_SELECT_ITEM.Name))
            {
                this.cboKishu.DisplayMember = Def_M_SELECT_ITEM.ITEM_NAME;
                this.cboKishu.ValueMember = Def_M_SELECT_ITEM.ITEM_NAME;
                this.cboKishu.DataSource = dtKishu.Tables[Def_M_SELECT_ITEM.Name];
            }

            // --- 明細 ---
            // 納品先
            DataTable dtNohinSakit = conn.GetSelectItem(new CondT01(this.UserInfo) { SelectGroupCode = SELECT_GROUP_CD.NOUHIN_SAKI_VALUE1 }).Tables[Def_M_SELECT_ITEM.Name];
            //this.shtMeisai.Columns[(int)SHEET_COL.NOUHIN_SAKI].Editor = this.CreateSuperiorComboEditor(dtNohinSakit, true, Def_M_SELECT_ITEM.ITEM_NAME, Def_M_SELECT_ITEM.ITEM_NAME, true);
            this.shtMeisai.Columns[(int)SHEET_COL.NOUHIN_SAKI].Editor = this.CreateSuperiorComboEditor(dtNohinSakit, true, Def_M_SELECT_ITEM.ITEM_NAME, true);

            // 出荷先
            DataTable dtSyukkaSaki = conn.GetSelectItem(new CondT01(this.UserInfo) { SelectGroupCode = SELECT_GROUP_CD.SYUKKA_SAKI_VALUE1 }).Tables[Def_M_SELECT_ITEM.Name];
            //this.shtMeisai.Columns[(int)SHEET_COL.SYUKKA_SAKI].Editor = this.CreateSuperiorComboEditor(dtSyukkaSaki, true, Def_M_SELECT_ITEM.ITEM_NAME, Def_M_SELECT_ITEM.ITEM_NAME, true);
            this.shtMeisai.Columns[(int)SHEET_COL.SYUKKA_SAKI].Editor = this.CreateSuperiorComboEditor(dtSyukkaSaki, true, Def_M_SELECT_ITEM.ITEM_NAME, true);

            // 手配区分
            this.shtMeisai.Columns[(int)SHEET_COL.TEHAI_FLAG].Editor = this.CreateSuperiorComboEditor(this.GetCommon(TEHAI_FLAG.GROUPCD).Tables[Def_M_COMMON.Name],
                false, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.VALUE1, false);

            // 数量単位
            this.shtMeisai.Columns[(int)SHEET_COL.QUANTITY_UNIT].Editor = this.CreateSuperiorComboEditor(this.GetCommon(QUANTITY_UNIT.GROUPCD).Tables[Def_M_COMMON.Name],
                false, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.VALUE1, false);

            // 返却品→出荷制限
            this.shtMeisai.Columns[(int)SHEET_COL.HENKYAKUHIN_FLAG].Editor = this.CreateSuperiorComboEditor(this.GetCommon(HENKYAKUHIN_FLAG.GROUPCD).Tables[Def_M_COMMON.Name],
                false, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.VALUE1, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細用コンボボックス作成(データバインド)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isTopEmpty"></param>
        /// <param name="displyMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="isEditable"></param>
        /// <returns></returns>
        /// <create>D.Naito 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor CreateSuperiorComboEditor(DataTable dt, bool isTopEmpty, string displyMember, string valueMember, bool isEditable)
        {
            try
            {
                if (isTopEmpty)
                    dt.Rows.InsertAt(dt.NewRow(), 0);
                var cboEditor = ElTabelleSheetHelper.NewSuperiorComboEditor(dt, displyMember, valueMember, isEditable);
                cboEditor.ValueAsIndex = false;
                return cboEditor;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細用コンボボックス作成(非データバインド)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isTopEmpty"></param>
        /// <param name="displyMember"></param>
        /// <param name="member"></param>
        /// <param name="isEditable"></param>
        /// <returns></returns>
        /// <create>D.Naito 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor CreateSuperiorComboEditor(DataTable dt, bool isTopEmpty, string member, bool isEditable)
        {
            try
            {
                var cboEditor = ElTabelleSheetHelper.NewSuperiorComboEditor();
                if (isTopEmpty)
                    cboEditor.Items.Add(new ComboItem(0, null, string.Empty, string.Empty, string.Empty));
                foreach (DataRow dr in dt.Rows)
                {
                    cboEditor.Items.Add(new ComboItem(0, null, ComFunc.GetFld(dr, member), string.Empty, ComFunc.GetFld(dr, member)));
                }
                cboEditor.Editable = isEditable;
                cboEditor.ValueAsIndex = false;
                return cboEditor;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細用コンボボックスアイテム追加(現在表示しているテキストがリストに存在しない場合)
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="row">行数</param>
        /// <returns>コンボボックス</returns>
        /// <create>D.Naito 2018/12/04</create>
        /// <update>N.Ikari 2022/05/25 出荷先リスト追加なし</update>
        /// --------------------------------------------------
        private void AddNoneItemSuperiorComboEditor(Sheet sheet, int row)
        {
            try
            {
                // 納品先
                if (sheet[(int)SHEET_COL.NOUHIN_SAKI, row].Value == null)
                {
                    SuperiorComboEditor cbo = sheet[(int)SHEET_COL.NOUHIN_SAKI, row].Editor as SuperiorComboEditor;
                    cbo = AddItemSuperiorComboEditor(cbo, sheet[(int)SHEET_COL.NOUHIN_SAKI, row].Text);
                    sheet[(int)SHEET_COL.NOUHIN_SAKI, row].Value = sheet[(int)SHEET_COL.NOUHIN_SAKI, row].Text;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細用コンボボックスアイテム追加
        /// </summary>
        /// <param name="cbo">コンボボックス</param>
        /// <param name="addValue">追加文字列</param>
        /// <returns>コンボボックス</returns>
        /// <create>D.Naito 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor AddItemSuperiorComboEditor(SuperiorComboEditor cbo, string addValue)
        {
            try
            {
                // cbo.Items.Contains() / cbo.Items.IndexOf()ではヒットしないので自前で検索
                bool isExists = false;
                foreach (ComboItem item in cbo.Items)
                {
                    if (item.Value.ToString().Equals(addValue))
                    {
                        isExists = true;
                        break;
                    }
                }
                if (!isExists)
                    cbo.Items.Add(new ComboItem(0, null, addValue, string.Empty, addValue));
                return cbo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>S.Furugo 2018/11/02</create>
        /// <update>Y.Shioshi 2022/04/11 F11ボタン追加</update>
        /// <update>H.Iimuro 2022/10/19 チェックボックス選択用ボタン追加</update>
        /// --------------------------------------------------
        private void ChangeMode(DisplayMode mode)
        {
            try
            {
                bool isEdiable = false;
                bool canUpdate = false;
                switch (mode)
                {
                    case DisplayMode.Initialize:
                        // ----- 初期 -----
                        this.grpSearch.Enabled = true;
                        this.tblEcsDetail.Enabled = false;
                        this.btnAllSelect.Enabled = false;
                        this.btnAllRelease.Enabled = false;
                        this.btnRangeSelect.Enabled = false;
                        this.btnRangeRelease.Enabled = false;
                        this.shtMeisai.Enabled = false;
                        this.fbrFunction.F08Button.Enabled = false;
                        isEdiable = false;
                        canUpdate = false;
                        this.EditMode = SystemBase.EditMode.None;   // 初期状態はNoneで仮設定
                        break;
                    case DisplayMode.Insert:
                        // ----- 新規(開始ボタン押下後) -----
                        this.grpSearch.Enabled = false;
                        this.tblEcsDetail.Enabled = true;
                        this.btnAllSelect.Enabled = true;
                        this.btnAllRelease.Enabled = true;
                        this.btnRangeSelect.Enabled = true;
                        this.btnRangeRelease.Enabled = true;
                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = true;
                        this.shtMeisai.AllowUserToDeleteRows = true;
                        this.shtMeisai.EditType = EditType.Default;
                        this.fbrFunction.F08Button.Enabled = false;
                        isEdiable = true;
                        canUpdate = true;
                        this.EditMode = SystemBase.EditMode.Insert;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更(開始ボタン押下後) -----
                        this.grpSearch.Enabled = false;
                        this.tblEcsDetail.Enabled = true;
                        this.btnAllSelect.Enabled = true;
                        this.btnAllRelease.Enabled = true;
                        this.btnRangeSelect.Enabled = true;
                        this.btnRangeRelease.Enabled = true;
                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = true;
                        this.shtMeisai.AllowUserToDeleteRows = true;
                        this.shtMeisai.EditType = EditType.Default;
                        this.fbrFunction.F08Button.Enabled = true;
                        isEdiable = true;
                        canUpdate = true;
                        this.EditMode = SystemBase.EditMode.Update;
                        break;
                    case DisplayMode.Delete:
                        // ----- 削除(開始ボタン押下後) -----
                        this.grpSearch.Enabled = false;
                        this.tblEcsDetail.Enabled = false;
                        this.btnAllSelect.Enabled = true;
                        this.btnAllRelease.Enabled = true;
                        this.btnRangeSelect.Enabled = true;
                        this.btnRangeRelease.Enabled = true;
                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = false;
                        this.shtMeisai.AllowUserToDeleteRows = false;
                        this.fbrFunction.F08Button.Enabled = true;
                        // this.shtMeisai.EditType = EditType.ReadOnly;
                        for (int row = 0; row < this.shtMeisai.Rows.Count; row++)
                            this.shtMeisai.Rows[row].Enabled = false;
                        isEdiable = false;
                        canUpdate = true;
                        this.EditMode = SystemBase.EditMode.Delete;
                        break;
                    default:
                        return;
                }
                this.fbrFunction.F01Button.Enabled = canUpdate;
                this.fbrFunction.F02Button.Enabled = isEdiable;
                this.fbrFunction.F03Button.Enabled = isEdiable;
                this.fbrFunction.F04Button.Enabled = isEdiable;
                //this.fbrFunction.F05Button.Enabled = isEdiable;
                this.fbrFunction.F06Button.Enabled = (isEdiable || canUpdate) && !this._dialogMode;
                this.fbrFunction.F07Button.Enabled = !this._dialogMode;
                //this.fbrFunction.F08Button.Enabled = isEdiable;
                this.fbrFunction.F09Button.Enabled = true;          //【F9】手配取込は、常時有効
                this.fbrFunction.F10Button.Enabled = isEdiable;
                this.fbrFunction.F11Button.Enabled = isEdiable;
#if __DEBUG_ENABLE_CODE__
                this.fbrFunction.F11Button.Text = "行Copy";
                this.fbrFunction.F11Button.Enabled = isEdiable;
#endif
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <param name="isFinished">管理フラグ完了状態</param>
        /// <create>D.Naito 2018/11/29</create>
        /// <update>Y.Shioshi 2022/04/11 F11ボタン追加</update>
        /// --------------------------------------------------
        private void ChangeMode(DisplayMode mode, bool isFinished)
        {
            this.ChangeMode(mode);
            if (isFinished)
            {
                this.grpSearch.Enabled = false;
                this.tblEcsDetail.Enabled = false;
                this.shtMeisai.Enabled = true;
                this.shtMeisai.AllowUserToAddRows = false;
                this.shtMeisai.AllowUserToDeleteRows = false;
                // this.shtMeisai.EditType = EditType.ReadOnly;
                for (int row = 0; row < this.shtMeisai.Rows.Count; row++)
                    this.shtMeisai.Rows[row].Enabled = false;

                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;
                this.fbrFunction.F03Button.Enabled = false;
                this.fbrFunction.F04Button.Enabled = false;
                this.fbrFunction.F05Button.Enabled = false;
                this.fbrFunction.F06Button.Enabled = false;
                this.fbrFunction.F08Button.Enabled = false;
                this.fbrFunction.F09Button.Enabled = true;      //【F9】手配取込は、常時有効
                this.fbrFunction.F11Button.Enabled = false;
            }
        }

        #endregion

        #region CHECK列のチェックボックスON/OFF制御用（フィルタリングを考慮）

        /// --------------------------------------------------
        /// <summary>
        /// 全選択・全選択解除時のCHECK列チェックボックスON/OFF設定
        /// </summary>
        /// <param name="isChecked">CHECK列チェックボックスをONするかどうか</param>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update>J.Chen 2022/11/07 全削除の場合、最終行選択を含む</update>
        /// --------------------------------------------------
        private void SetAllCheck(bool isChecked)
        {
            if (rdoAllDelete.Checked)
            {
                var ranges = this.GetValidRanges(new GrapeCity.Win.ElTabelle.Range[] { new GrapeCity.Win.ElTabelle.Range(SHEET_COL_CHECK, 0, SHEET_COL_CHECK, this.shtMeisai.MaxRows - 1) });
                this.SetCellRangeValueForCheckBox(ranges, isChecked);
            }
            else
            {
                var ranges = this.GetValidRanges(new GrapeCity.Win.ElTabelle.Range[] { new GrapeCity.Win.ElTabelle.Range(SHEET_COL_CHECK, 0, SHEET_COL_CHECK, this.shtMeisai.MaxRows - 2) });
                this.SetCellRangeValueForCheckBox(ranges, isChecked);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択・範囲選択解除時のCHECK列チェックボックスON/OFF設定
        /// </summary>
        /// <param name="isChecked">CHECK列チェックボックスをONするかどうか</param>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetRangeCheck(bool isChecked)
        {
            var ranges = this.GetValidRanges(this.shtMeisai.GetBlocks(GrapeCity.Win.ElTabelle.BlocksType.Selection));
            this.SetCellRangeValueForCheckBox(ranges, isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定Rangeのチェックボックス値設定
        /// </summary>
        /// <param name="ranges">Range配列</param>
        /// <param name="isCheck">チェックボックスをONするかどうか</param>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetCellRangeValueForCheckBox(GrapeCity.Win.ElTabelle.Range[] ranges, bool isChecked)
        {
            foreach (var range in ranges)
            {
                this.shtMeisai.CellRange = range;
                this.shtMeisai.CellValue = isChecked ? CHECK_ON : CHECK_OFF;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定Rangeで有効となっているRange配列を取得
        /// </summary>
        /// <param name="ranges">Range配列</param>
        /// <returns></returns>
        /// <create>H.Iimuro 2022/10/19</create>
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
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsValidRow(int rowIndex)
        {
            if (this.shtMeisai.MaxRows - 1 < rowIndex)
            {
                return false;
            }

            if (this.shtMeisai.Rows[rowIndex].Height < 1)
            {
                return false;            }

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// チェックがついている数を取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private int CheckCount()
        {
            if (this.shtMeisai.MaxRows <= 0) return 0;
            int ret = 0;
            for (int i = 0; i < this.shtMeisai.MaxRows; i++)
            {
                if (this.shtMeisai.Columns[SHEET_COL_CHECK].ValueBlock[i] != null)
                {
                    if (Convert.ToInt32(this.shtMeisai.Columns[SHEET_COL_CHECK].ValueBlock[i]) == CHECK_ON) ret++;
                }
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// チェックがついている行番号を取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private ArrayList CheckIndex() 
        {
            ArrayList indexList = new ArrayList();
            for (int i = 0; i < this.shtMeisai.MaxRows; i++)
            {
                // 未選択はスキップ
                var checkstate = this.shtMeisai[SHEET_COL_CHECK, i].Value;
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

        #region インポート

        /// --------------------------------------------------
        /// <summary>
        /// 図番/形式へデータを反映する
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="list">図番/形式一覧</param>
        /// <create>D.Okumura 2018/12/19</create>
        /// <update></update>
        /// <remarks>
        /// catchは上位で行うこと
        /// </remarks>
        /// --------------------------------------------------
        private void AddXlsDataZubanKeishiki(Sheet sheet, IEnumerable<string> list)
        {
            try
            {
                sheet.Redraw = false;
                int currentRow = sheet.Rows.Count - 1;
                sheet.InsertRow(currentRow, list.Count(), false);
                int i = 0;
                foreach (var item in list)
                {
                    // 初期データを反映
                    SetSheetRowInitData(sheet, currentRow + i);
                    // データを列に設定
                    sheet[(int)SHEET_COL.ZUMEN_KEISHIKI, currentRow + i].Value = item.TrimEnd('\r', '\n');
                    i++;
                }
            }
            finally
            {
                sheet.Redraw = true;
            }
        }

#endregion

        #region インポート　2022/10/19 【Step15】

        /// --------------------------------------------------
        /// <summary>
        /// 図番/形式へデータを反映する
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="list">図番/形式一覧</param>
        /// <create>D.Okumura 2018/12/19</create>
        /// <update>TW-Tsuji 2022/10/19</update>
        /// <remarks>
        /// catchは上位で行うこと
        /// </remarks>
        /// <remarks>
        /// 　2022/10/19 元のファンクションを改修して作成した。
        /// 　　・取込Excelの仕様変更に伴い、データ構造の変更
        /// </remarks>
        /// --------------------------------------------------
        private void AddXlsDataZubanKeishikiEx(Sheet sheet, CondT01 header, DataTable meisai)
        {
            try
            {
                //【Step15】手配取込Excel様式の拡張と変更 2022/10/19（TW-Tsuji）
                //
                this.SuspendLayout();
                sheet.Redraw = false;

                //　物件名
                this.cboBukkenName.SelectedIndex = -1;
                if(string.IsNullOrEmpty(header.BukkenName))
                {
                    this.cboBukkenName.SelectedValue = "";
                }
                else
                {
                    this.cboBukkenName.SelectedValue = header.BukkenName;
                }

                //　製番
                this.txtSeiban.Text = header.Seiban;

                //　CODE
                this.txtCode.Text = header.Code;

                //　機種
                this.cboKishu.SelectedIndex = -1;
                this.cboKishu.SelectedValue = "";
                if (!string.IsNullOrEmpty(header.DispSelect))       //【注】プロパティ DispSelectを使用
                {
                    this.cboKishu.SelectedValue = header.DispSelect;
                }
                
                //　AR No.
                this.txtARNo.Text = header.ARNo;

                //　シート明細の表示
                int currentRow = sheet.Rows.Count - 1;
                sheet.InsertRow(currentRow, meisai.Rows.Count, false);
                int i = 0;
                sheet.Redraw = false;
                //this.shtMeisai.Redraw = false;
                foreach (DataRow r in meisai.Rows)
                {
                    //初期データを反映
                    SetSheetRowInitData(sheet, currentRow + i);
                    //sheet.Redraw = false;
                   //データを列に設定
                    sheet[(int)SHEET_COL.SETTEI_DATE, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.SETTEI_DATE];            //設定納期 
                    sheet[(int)SHEET_COL.NOUHIN_SAKI, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.NOUHIN_SAKI];            //納品先
                    sheet[(int)SHEET_COL.SYUKKA_SAKI, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.SYUKKA_SAKI];            //出荷先
                    sheet[(int)SHEET_COL.FLOOR, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.FLOOR];                        //Floor
                    sheet[(int)SHEET_COL.ST_NO, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.ST_NO];                        //ST-No.
                    sheet[(int)SHEET_COL.ZUMEN_OIBAN, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.ZUMEN_OIBAN];            //追番
                    sheet[(int)SHEET_COL.HINMEI_JP, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.HINMEI_JP];                //品名（和文）
                    sheet[(int)SHEET_COL.HINMEI, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.HINMEI];                      //品名
                    sheet[(int)SHEET_COL.ZUMEN_KEISHIKI, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI];      //図番/形式
                    //this.AssistInputZumenKeishikiImp(sheet, currentRow + i, 
                    //    r[Def_T_TEHAI_MEISAI.HINMEI_JP].ToString(), r[Def_T_TEHAI_MEISAI.HINMEI].ToString());               //図番/形式の入力補助
                    sheet[(int)SHEET_COL.ZUMEN_KEISHIKI2, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2];    //図番/形式2
                    //this.AssistInputZumenKeishiki2(sheet, currentRow + i);                                                      //図番/形式2の入力補助
                    sheet[(int)SHEET_COL.TEHAI_QTY, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.TEHAI_QTY];                //手配数
                    sheet[(int)SHEET_COL.QUANTITY_UNIT, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.QUANTITY_UNIT];        //数量単位
                    sheet[(int)SHEET_COL.TEHAI_FLAG, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.TEHAI_FLAG];              //手配区分
                    this.ChangeTehaiFlag(sheet, currentRow + i, r[Def_T_TEHAI_MEISAI.TEHAI_FLAG].ToString(), true);             //手配区分の入力補助
                    // 発注の場合、発注数と出荷数の入力補助を有効にします
                    if (sheet[(int)SHEET_COL.TEHAI_FLAG, currentRow + i].Value.ToString() == TEHAI_FLAG.ORDERED_VALUE1)
                    {
                        sheet[(int)SHEET_COL.HACCHU_QTY, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.TEHAI_QTY];                //発注数入力補助
                        sheet[(int)SHEET_COL.SHUKKA_QTY, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.TEHAI_QTY];                //出荷数入力補助
                    }

                    // SKS Skipの場合、出荷数の入力補助を有効にします
                    if (sheet[(int)SHEET_COL.TEHAI_FLAG, currentRow + i].Value.ToString() == TEHAI_FLAG.SKS_SKIP_VALUE1)
                    {
                        sheet[(int)SHEET_COL.SHUKKA_QTY, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.TEHAI_QTY];                //出荷数入力補助
                    }

                    sheet[(int)SHEET_COL.TEHAI_NO, currentRow + i].Value = r[Def_T_TEHAI_SKS.TEHAI_NO];                     //手配No
                    sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, currentRow + i].Value = r[Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG];  //返却品
                    this.ChangeHenkyakuhinFlag(sheet, currentRow + 1, true);
                    
                    i++;
                }
            }
            finally
            {
                sheet.Redraw = true;
                this.ResumeLayout();
            }
        }

        #endregion

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディションの取得
        /// </summary>
        /// <returns>コンディション</returns>
        /// <create>S.Furugo 2018/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondT01 GetCondition()
        {
            CondT01 cond = new CondT01(this.UserInfo);

            cond.EcsQuota = this.txtEcsQuota.Text;
            cond.EcsNo = this.txtEcsNo.Text;

            return cond;
        }

        #endregion

        #region セルコピー処理

        /// --------------------------------------------------
        /// <summary>
        /// 1行上のセル値をコピペ
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="row">コピー先行</param>
        /// <param name="columns">コピー列</param>
        /// <create>H.Tsuji 2019/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void copyCell(Sheet sheet, int row, int col)
        {
            int srcRow = row - 1;
            if (srcRow < 0 || row < 1 || col < 0) return;

            // 「Assy no.」列の場合はコピー元が無効状態でもコピーを許可する
            if (sheet[col, row].Enabled
                && (sheet[col, srcRow].Enabled || col.Equals((int)SHEET_COL.ASSY_NO)))
            {
                sheet[col, row].Value = sheet[col, srcRow].Value;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 1行上のセル値をコピペ
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="row">コピー先行</param>
        /// <param name="columns">コピー列</param>
        /// <create>D.Naito 2018/11/27</create>
        /// <update>K.Tsutsumi 2019/03/08 コピー元行の対応</update>
        /// --------------------------------------------------
        private void copyCell(Sheet sheet, int row, params int[] columns)
        {
            this.copyCell(sheet, row - 1, row, columns);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定行１のセル値を指定行２へコピペ
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="srcRow">コピー元行</param>
        /// <param name="dstRow">コピー先行</param>
        /// <param name="columns">コピー列</param>
        /// <create>K.Tsutsumi 2019/03/08 コピー元行の対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private void copyCell(Sheet sheet, int srcRow, int dstRow, params int[] columns)
        {
            if (srcRow < 0 || dstRow < 1 || columns == null) return;

            foreach (int col in columns)
            {
                if (sheet[col, dstRow].Enabled && sheet[col, srcRow].Enabled)
                {
                    sheet[col, dstRow].Value = sheet[col, srcRow].Value;
                }
            }
        }

        #endregion

        #region 登録データ取得
        /// --------------------------------------------------
        /// <summary>
        /// 技連情報取得
        /// </summary>
        /// <returns>技連情報テーブル</returns>
        /// <create>D.Naito 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetDataGiren()
        {
            try{
                DataTable dt = CreateSchemaGiren();
                DataRow dr = dt.NewRow();
                if (rdoInsert.Checked)
                {
                    //--- 新規 ---
                    dr[Def_M_ECS.KANRI_FLAG] = KANRI_FLAG.MIKAN_VALUE1;
                }
                else if (rdoChange.Checked || rdoAllDelete.Checked)
                {
                    //--- 編集 || 削除 ---
                    // 管理フラグは更新対象外
                    dr[Def_M_ECS.VERSION] = this._girenVerOrg;  // 削除・編集時は検索時のバージョンを使用
                }

                dr[Def_M_ECS.ECS_QUOTA] = this.txtEcsQuota.Text;
                dr[Def_M_ECS.ECS_NO] = this.txtEcsNo.Text;
                dr[Def_M_ECS.PROJECT_NO] = this.cboBukkenName.SelectedValue;
                dr[Def_M_ECS.SEIBAN] = this.txtSeiban.Text;
                dr[Def_M_ECS.CODE] = this.txtCode.Text;
                dr[Def_M_ECS.KISHU] = string.IsNullOrEmpty(this.cboKishu.Text) ? null : this.cboKishu.Text;
                dr[Def_M_ECS.SEIBAN_CODE] = string.Concat(this.txtSeiban.Text, this.txtCode.Text);
                dr[Def_M_ECS.AR_NO] = string.IsNullOrEmpty(this.txtARNo.Text) ? null : lblAR.Text + this.txtARNo.Text;
                dt.Rows.Add(dr);

                return dt;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細情報取得(フィルター抽出)
        /// </summary>
        /// <param name="dtSrc">取得元明細情報</param>
        /// <param name="state">抽出条件</param>
        /// <returns>手配明細テーブル</returns>
        /// <create>D.Naito 2018/11/26</create>
        /// <update>J.Chen 2022/05/18 STEP14</update>
        /// <update>J.Chen 2024/11/06 更新対象を絞り込む</update>
        /// --------------------------------------------------
        private DataTable GetDataTehaiMeisaiFilter(DataTable dtSrc, DataRowState state)
        {
            try
            {
                DataTable dt = null;
                string name = string.Empty;

                switch (state)
                {
                    case DataRowState.Added:
                        dt = dtSrc.GetChanges(DataRowState.Added);
                        if (dt == null) return null;

                        // ====================================================================================================================
                        // 行入れ替え処理の仕様、GetChangesメソッドで一部の行の変化うまく取得できないため追加する
                        // 新規追加の場合手配NoがNullなので、それ以外削除する
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (!dt.Rows[i].IsNull(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO))
                            {
                                dt.Rows[i].Delete();
                            }
                        }
                        // ====================================================================================================================

                        setColumnsField(dt, Def_T_TEHAI_MEISAI.ECS_QUOTA, txtEcsQuota.Text);
                        setColumnsField(dt, Def_T_TEHAI_MEISAI.ECS_NO, txtEcsNo.Text);
                        name = ComDefine.DTTBL_INSERT;
                        break;
                    case DataRowState.Modified:
                        dt = dtSrc.GetChanges(DataRowState.Modified);

                        // ====================================================================================================================
                        // 行入れ替え処理の仕様、GetChangesメソッドで一部の行の変化うまく取得できないため追加する
                        // 更新する行を取得する
                        DataTable dtTemp = dtSrc.GetChanges(DataRowState.Added);
                        if (dtTemp != null)
                        {
                            for (int i = dtTemp.Rows.Count - 1; i >= 0; i--)
                            {
                                if (dtTemp.Rows[i].IsNull(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO))
                                {
                                    dtTemp.Rows[i].Delete();
                                }
                                else
                                {
                                    if (dt == null)
                                    {
                                        dt = this.GetSchemaTehaiMeisai();
                                    }
                                    dt.ImportRow(dtTemp.Rows[i]);
                                }
                            }
                        }
                        // ====================================================================================================================

                        if (dt == null) return null;
                        name = ComDefine.DTTBL_UPDATE;

                        // 差分を保存用
                        DataTable resultTable = dt.Clone();

                        // dtの行をループ処理
                        foreach (DataRow currentRow in dt.Rows)
                        {
                            var matchingRows = _updCheckDt.Select(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO + " = '" + currentRow[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO] + "'");

                            if (matchingRows.Length > 0)
                            {
                                DataRow backupRow = matchingRows[0];

                                bool hasChanges = false;

                                // 列の差分を確認（インデックスを使用）
                                for (int i = 2; i < dt.Columns.Count; i++)
                                {
                                    if (i == 4) // 設定納期場合は日付部分だけを比較
                                    {
                                        DateTime currentDate = Convert.ToDateTime(currentRow[i]).Date;
                                        DateTime backupDate = Convert.ToDateTime(backupRow[i]).Date;

                                        if (!Equals(currentDate, backupDate))
                                        {
                                            hasChanges = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (!Equals(currentRow[i], backupRow[i]))
                                        {
                                            hasChanges = true;
                                            break;
                                        }
                                    }
                                }

                                // 差分がある場合、テーブルに行を追加
                                if (hasChanges)
                                {
                                    resultTable.ImportRow(currentRow);
                                }
                            }
                        }

                        dt = resultTable.Copy();

                        break;
                    case DataRowState.Deleted:
                        DataView dv =  dtSrc.Copy().DefaultView;
                        dv.RowStateFilter = DataViewRowState.Deleted;
                        if (dv == null) return null;
                        dt =dtSrc.Clone();

                        for (int i = 0; i < dv.Count; i++)
                        {
                            DataRow dr = dt.NewRow();
                            for(int j = 0; j < dt.Columns.Count; j++)
                            {
                                dr[j] = dv[i][j];
                            }
                            dt.Rows.Add(dr);
                        }
                        name = ComDefine.DTTBL_DELETE;
                        break;
                    default:
                        dt = dtSrc.Clone();
                        break;
                }
                dt.TableName = name;

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// テーブル列への値設定
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="field">列名</param>
        /// <param name="value">値</param>
        /// <create>D.Naito 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void setColumnsField(DataTable dt, string field, object value)
        {
            foreach (DataRow row in dt.Rows)
                row[field] = value;
        }

        #endregion

        #region テーブルデータ作成
        /// --------------------------------------------------
        /// <summary>
        /// 手配明細データテーブル作成
        /// </summary>
        /// <returns>データテーブル</returns>
        /// <create>S.Furugo 2018/11/06</create>
        /// <update>H.Tsuji 2019/08/26 STEP10 組立パーツの識別処理を追加</update>
        /// <update>T.Nukaga 2019/11/15 STEP12 返却品管理対応 HENKYAKU_FLAG追加</update>
        /// <update>J.Chen 2022/10/31 手配No追加</update>
        /// <update>J.Chen 2024/10/23 履歴更新列追加</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// --------------------------------------------------
        private DataTable GetSchemaTehaiMeisai()
        {
            try
            {
                DataTable dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
                dt.Columns.Add(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ECS_QUOTA, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ECS_NO, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.SETTEI_DATE, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.NOUHIN_SAKI, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.SYUKKA_SAKI, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ZUMEN_OIBAN, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.FLOOR, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ST_NO, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.HINMEI_JP, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.HINMEI, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.HINMEI_INV, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI_S, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.TEHAI_QTY, typeof(decimal));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.TEHAI_FLAG, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_SKS.TEHAI_NO, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.TEHAI_KIND_FLAG, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.HACCHU_QTY, typeof(decimal));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.SHUKKA_QTY, typeof(decimal));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ASSY_NO, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.FREE1, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.FREE2, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.QUANTITY_UNIT, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.NOTE, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.NOTE2, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.NOTE3, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.CUSTOMS_STATUS, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.MAKER, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.UNIT_PRICE, typeof(decimal));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ARRIVAL_QTY, typeof(decimal));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ASSY_QTY, typeof(decimal));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ESTIMATE_FLAG, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.ESTIMATE_NO, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.CREATE_DATE, typeof(DateTime));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.CREATE_USER_ID, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.CREATE_USER_NAME, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.UPDATE_DATE, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.UPDATE_USER_ID, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.UPDATE_USER_NAME, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.VERSION, typeof(string));
                dt.Columns.Add(ComDefine.FLD_SHUKKA_MEISAI_CNT, typeof(decimal));
                dt.Columns.Add(ComDefine.FLD_SHUKKA_MEISAI_QTY, typeof(decimal));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.DISP_NO, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_MEISAI.TEHAI_RIREKI, typeof(string));

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 技連マスタデータテーブル作成
        /// </summary>
        /// <returns>技連マスタデータテーブル</returns>
        /// <create>D.Naito 2018/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable CreateSchemaGiren()
        {
            try
            {
                DataTable dt = new DataTable(Def_M_ECS.Name);
                dt.Columns.Add(Def_M_ECS.ECS_QUOTA, typeof(string));
                dt.Columns.Add(Def_M_ECS.ECS_NO, typeof(string));
                dt.Columns.Add(Def_M_ECS.PROJECT_NO, typeof(string));
                dt.Columns.Add(Def_M_ECS.SEIBAN, typeof(string));
                dt.Columns.Add(Def_M_ECS.CODE, typeof(string));
                dt.Columns.Add(Def_M_ECS.KISHU, typeof(string));
                dt.Columns.Add(Def_M_ECS.SEIBAN_CODE, typeof(string));
                dt.Columns.Add(Def_M_ECS.AR_NO, typeof(string));
                dt.Columns.Add(Def_M_ECS.KANRI_FLAG, typeof(string));
                dt.Columns.Add(Def_M_ECS.CREATE_DATE, typeof(string));
                dt.Columns.Add(Def_M_ECS.CREATE_USER_ID, typeof(string));
                dt.Columns.Add(Def_M_ECS.CREATE_USER_NAME, typeof(string));
                dt.Columns.Add(Def_M_ECS.UPDATE_DATE, typeof(string));
                dt.Columns.Add(Def_M_ECS.UPDATE_USER_ID, typeof(string));
                dt.Columns.Add(Def_M_ECS.UPDATE_USER_NAME, typeof(string));
                dt.Columns.Add(Def_M_ECS.MAINTE_DATE, typeof(string));
                dt.Columns.Add(Def_M_ECS.MAINTE_USER_ID, typeof(string));
                dt.Columns.Add(Def_M_ECS.MAINTE_USER_NAME, typeof(string));
                dt.Columns.Add(Def_M_ECS.VERSION, typeof(DateTime));

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 手配SKS連携データテーブル作成
        /// </summary>
        /// <returns>データテーブル</returns>
        /// <create>J.Chen 2022/10/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemaTehaiMeisaiSKS(String TableName)
        {
            try
            {
                DataTable dt = new DataTable(TableName);
                dt.Columns.Add(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_SKS.TEHAI_NO, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_SKS.KENPIN_UMU, typeof(string));
                dt.Columns.Add(Def_T_TEHAI_SKS_WORK.CREATE_DATE, typeof(object));

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Assy no.採番処理

        /// --------------------------------------------------
        /// <summary>
        /// Assy no.採番処理
        /// </summary>
        /// <param name="assyNo">採番したAssy no.</param>
        /// <param name="errMsgID">エラーメッセージID</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetSaibanAssyNo(out string assyNo, out string errMsgID)
        {
            try
            {
                ConnSms connSms = new ConnSms();
                CondSms cond = new CondSms(this.UserInfo)
                {
                    SaibanFlag = SAIBAN_FLAG.ASSY_NO_VALUE1,
                };
                return connSms.GetSaiban(cond, out assyNo, out errMsgID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 返却品フラグ切替時処理
        /// --------------------------------------------------
        /// <summary>
        /// 返却品フラグ切替時処理
        /// </summary>
        /// <create>T.Nukaga 2019/12/16 STEP12 返却品管理対応</create>
        /// <param name="sheet">シート</param>
        /// <param name="row">行数</param>
        /// <param name="chkAssyNo">AssyNo入力補助呼び出し有無フラグ</param>
        /// <update>N.Ikari 2022/05/11 返品フラグ対象時の値を0に設定</update>
        /// <update>Y.Gwon 2023/07/04 STEP16 出荷数を手配数と同数を設定</update>
        /// <update>Y.Gwon 2023/07/31 返却品の区分が「返却対象」の場合、該当行を紫色にする処理</update>
        /// <update>J.Chen 2023/09/28 返却品の区分「保留」「通関確認中」は「返却対象」と同じ処理とする</update>
        /// <update>J.Chen 2023/10/13 明細分割時、返却フラグ変更による出荷数の修正補助を行わないようにする</update>
        /// --------------------------------------------------
        private void ChangeHenkyakuhinFlag(Sheet sheet, int row, bool chkAssyNo)
        {
            // 値が未設定の場合はなにもしない(新規行追加時)
            if (sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Value == null)
            {
                return;
            }

            try
            {
                // 再描画を一時停止
                sheet.Redraw = false;

                // 返却品フラグ対象時の入力補助(出荷数設定)
                if (sheet[(int)SHEET_COL.HENKYAKUHIN_FLAG, row].Value.ToString() != HENKYAKUHIN_FLAG.DEFAULT_VALUE1)
                {
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Enabled = false;
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value = 0;
                    SetupRowColor(this.shtMeisai.Rows[row], ComDefine.PURPLE_COLOR, this.shtMeisai.GridLine, Borders.All);
                }
                else
                {
                    sheet[(int)SHEET_COL.SHUKKA_QTY, row].Enabled = true;

                    // 返却対象以外の場合、出荷数を手配数と同じ値にする
                    if (Convert.ToInt32(sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value) == 0 && isHenkyakuhinFlagChange && !isMeisaiBunkatsu)
                    {
                        sheet[(int)SHEET_COL.SHUKKA_QTY, row].Value = sheet[(int)SHEET_COL.TEHAI_QTY, row].Value;
                    }

                    if (chkAssyNo)
                    {
                        // 手配区分がAssy以外でAssyNoに値が設定されている場合は入力不可
                        this.AssistInputAssyNo(sheet, row);
                    }
                }
            }
            finally
            {
                // 必ず再描画をかける
                sheet.Redraw = true;
            }
        }
        #endregion

        #region デバッグ用
        /// --------------------------------------------------
        /// <summary>
        /// デバッグ用WriteLine
        /// </summary>
        /// <param name="message">表示メッセージ</param>
        /// <param name="func">メソッド名</param>
        /// <create>D.Naito 2018/12/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void debugWriteLine(string func, string message)
        {
#if __DEBUG_ENABLE_CODE__
            System.Diagnostics.Debug.WriteLine("[" + DateTime.Now.ToString("MM/dd HH:mm:ss:fff") + " / " + 
                MethodBase.GetCurrentMethod().Module + " / "+ func + "]" + message);
#endif
        }

#if __DEBUG_ENABLE_CODE__
        /// --------------------------------------------------
        /// <summary>
        /// デバッグ用ハンドラー登録
        /// </summary>
        /// <create>D.Naito 2018/12/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void setDebugEventtHandler()
        {
            try
            {
                //this.shtMeisai.RowUpdating += new RowUpdatingEventHandler(this.debugRowUpdatingEventHandler);
                this.shtMeisai.FillingData += new System.EventHandler<FillingDataEventArgs>(this.debugFillingDataEventHandler);
                this.shtMeisai.RowInserting += new RowInsertingEventHandler(this.debugRowInsertingEventHandler);
                this.shtMeisai.RowNeededFilling += new RowNeedFillingEventHandler(this.debugRowNeedFillingHandler);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// デバッグ用ハンドラー
        /// </summary>
        /// <create>D.Naito 2018/12/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void debugRowUpdatingEventHandler(object sender, RowUpdatingEventArgs e)
        {
            debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");
        }
        private void debugFillingDataEventHandler(object sender, FillingDataEventArgs e)
        {
            debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");
        }
        private void debugRowInsertingEventHandler(object sender, RowInsertingEventArgs e)
        {
            debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");
        }
        private void debugRowNeedFillingHandler(object sender, RowNeedFillingEventArgs e)
        {
            debugWriteLine(MethodBase.GetCurrentMethod().Name, "Call");
        }
#endif
        #endregion

        #region 行入れ替え処理
        /// --------------------------------------------------
        /// <summary>
        /// データグリッドマウス移動処理(MouseMove)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2022/05/18 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_MouseMove(object sender, MouseEventArgs e)
        {
            // 右クリックの場合
            if (e.Button == MouseButtons.Right)
            {
                if (dragBox != Rectangle.Empty && !(dragBox.Contains(e.X, e.Y)) && sourceRowTemp > -1)
                {
                    DragDropEffects dropEffect = this.shtMeisai.DoDragDrop(this.shtMeisai.Rows[sourceRowTemp], DragDropEffects.Move);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データグリッドマウスダウン処理(MouseDown)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2022/05/18 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_MouseDown(object sender, MouseEventArgs e)
        {
            // 右クリックの場合（行全体選択）
            if (e.Button == MouseButtons.Right)
            {
                this.shtMeisai.ViewMode = ViewMode.Row;
            }
            else
            {
                this.shtMeisai.ViewMode = ViewMode.Default;
            }

            // 移動元（座標）
            Range objRange;
            if (this.shtMeisai.HitTest(new Point(e.X, e.Y), out objRange) == SheetArea.Cell)
            {
                // アクティブセルを変更する
                this.shtMeisai.ActivePosition = objRange.Position;

                // 移動元（行番号）
                sourceRow = new Position(objRange.LeftColumn, objRange.TopRow).Row;
            }
            else
            {
                // ヘッダー行の場合
                sourceRow = -1;
            }

            // ヘッダー以外&&最終行以外
            if (sourceRow >= -1 && sourceRow < this.shtMeisai.MaxRows - 1)
            {
                var dragSize = SystemInformation.DragSize;
                // ドラッグ操作が開始されない範囲を取得
                dragBox = new Rectangle(new Point(e.X - dragSize.Width / 2, e.Y - dragSize.Height / 2), dragSize);
            }
            else
            {
                dragBox = Rectangle.Empty;
            }

            // 画面上の移動先番号を保存
            sourceRowTemp = sourceRow;

            DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
            List<int> delIndex = new List<int>();

            // 削除した行番号を取得
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i].RowState == DataRowState.Deleted)
                {
                    delIndex.Add(i);
                }
            }

            // 実際データテーブルの移動元番号を取得
            for (int i = 0; i < delIndex.Count; i++)
            {
                if (delIndex[i] <= sourceRow)
                {
                    sourceRow++;
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データグリッドドラッグオ－バー処理(DragOver)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2022/05/18 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// --------------------------------------------------
        /// <summary>
        /// データグリッドドロップ処理(DragDrop)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2022/05/18 STEP14</create>
        /// <update>H.Iimuro 2022/10/20 並び替え時の背景色・チェックボックス追加</update>
        /// <update>H.Iimuro 2022/10/25 背景色グレーの追加</update>
        /// <update>Y.Gwon 2023/07/31 返却品の区分が「返却対象」の場合にも問題なく紫色を維持しながら並び替えられる</update>
        /// <update>J.Chen 2023/09/28 返却品の区分「保留」「通関確認中」は「返却対象」と同じ処理とする</update>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_DragDrop(object sender, DragEventArgs e)
        {
            // データグリッドのポイントを取得（移動先座標）
            Point clientPoint = this.shtMeisai.PointToClient(new Point(e.X, e.Y));

            // 移動先（行番号）
            Range objRange;
            if (this.shtMeisai.HitTest(clientPoint, out objRange) == SheetArea.Cell)
            {
                destinationRow = new Position(objRange.LeftColumn, objRange.TopRow).Row;
            }
            else
            {
                // ヘッダー行の場合
                destinationRow = -1;
            }

            // ドラッグ＆ドロップ実行中&&ヘッダー行範囲内
            if (e.Effect == DragDropEffects.Move && destinationRow > -1)
            {
                // 編集確定イベントを強制に発生させる
                this.shtMeisai.ActivePosition = new Position(0, 0);
                // 移動データ退避
                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                Object[] rowArray = dt.Rows[sourceRow].ItemArray; 
                DataRow row = dt.NewRow(); 
                row.ItemArray = rowArray;
                // チェックボックス選択状況の退避
                int firstIndex = destinationRow;
                int lastIndex = sourceRow;
                if (sourceRow < destinationRow)
                {
                    firstIndex = sourceRow;
                    lastIndex = destinationRow;
                }

                var checkOnIndex = new List<int>();
                var checkOffIndex = new List<int>();
                for (int i = firstIndex; i < lastIndex + 1; i++)
                {
                    if (this.shtMeisai[SHEET_COL_CHECK, i].Value != null && this.shtMeisai[SHEET_COL_CHECK, i].Value.ToString() == CHECK_ON_STR)
                    {
                        checkOnIndex.Add(i);
                    }
                    else 
                    {
                        checkOffIndex.Add(i);
                    }
                }
                int[] checkOnIndexs = new int[0];
                if(checkOnIndex != null)
                {
                    checkOnIndexs = checkOnIndex.ToArray();
                }
                int[] checkOffIndexs = new int[0];
                if (checkOffIndex != null)
                {
                    checkOffIndexs = checkOffIndex.ToArray();
                }

                // 削除した行番号を取得
                List<int> delIndex = new List<int>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].RowState == DataRowState.Deleted)
                    {
                        delIndex.Add(i);
                    }
                } 

                // 実際データテーブルの移動先番号を取得
                for (int i = 0; i < delIndex.Count; i++)
                {
                    if (delIndex[i] <= destinationRow)
                    {
                        destinationRow++;
                    }
                }

                // 行入れ替えON
                isRowChange = true;

                // 移動元削除
                dt.Rows.RemoveAt(sourceRow);
                // 移動先新規行挿入
                dt.Rows.InsertAt(row, destinationRow);

                this.ActiveControl = null;

                // 表示中の画面に反映する
                string sourceChecked = "";
                if (this.shtMeisai[SHEET_COL_CHECK, sourceRow].Value != null)
                {
                    sourceChecked = this.shtMeisai[SHEET_COL_CHECK, sourceRow].Value.ToString();
                }
                this.shtMeisai.DataSource = dt;

                // 背景色の更新
                firstIndex = destinationRow;
                lastIndex = sourceRow;
                bool mouseUpFlag = true;
                // 対象行を下に並び替え時
                if (sourceRow < destinationRow)
                {
                    firstIndex = sourceRow;
                    lastIndex = destinationRow;
                    mouseUpFlag = false;
                }
                for (int i = firstIndex; i < lastIndex + 1; i++)
                {
                    // 背景色
                    if ("0".Equals(this.shtMeisai[(int)SHEET_COL.SHUKKA_QTY, i].Value.ToString())
                            && this.shtMeisai[(int)SHEET_COL.HENKYAKUHIN_FLAG, i].Value.ToString() == HENKYAKUHIN_FLAG.DEFAULT_VALUE1)
                    {
                        //出荷数0の時、濃いグレーで表示します
                        var color = ComDefine.GRY_COLOR;
                        SetupRowColor(this.shtMeisai.Rows[i], color, this.shtMeisai.GridLine, Borders.All);
                    }
                    else if (this.shtMeisai[(int)SHEET_COL.HENKYAKUHIN_FLAG, i].Value.ToString() != HENKYAKUHIN_FLAG.DEFAULT_VALUE1)
                    {
                        //返却品の区分が「返却対象」の場合紫色で表示します
                        var color = ComDefine.PURPLE_COLOR;
                        SetupRowColor(this.shtMeisai.Rows[i], color, this.shtMeisai.GridLine, Borders.All);
                        //出荷数の修正は不可
                        this.shtMeisai[(int)SHEET_COL.SHUKKA_QTY, i].Enabled = false;
                    }
                    else if (this.shtMeisai[(int)SHEET_COL.HENKYAKUHIN_FLAG, i].Value.ToString() == HENKYAKUHIN_FLAG.DEFAULT_VALUE1)
                    {
                        //背景色を基本値に設定
                        defaultRowBackColor(this.shtMeisai.Rows[i]);
                        this.shtMeisai[(int)SHEET_COL.SHUKKA_QTY, i].Enabled = true;
                    }
                    
                    else if (this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value == null || this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value.ToString() == ComDefine.NEUTRAL_FLAG)
                    {
                        defaultRowBackColor(this.shtMeisai.Rows[i]);
                    }
                    else if (this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value.ToString() == ComDefine.ONEROUS_FLAG)
                    {
                        SetupRowColor(this.shtMeisai.Rows[i], ComDefine.ONEROUS_COLOR, this.shtMeisai.GridLine, Borders.All);
                    }
                    else if (this.shtMeisai[SHEET_COL_ESTIMATE_FLAG, i].Value.ToString() == ComDefine.GRATIS_FLAG)
                    {
                        SetupRowColor(this.shtMeisai.Rows[i], ComDefine.GRATIS_COLOR, this.shtMeisai.GridLine, Borders.All);
                    }
                }
                // チェックボックス　並び替え
                if (sourceChecked == CHECK_ON_STR)
                {
                    if (mouseUpFlag)
                    {
                        for (int i = 0; i < checkOnIndexs.Length; i++)
                        {
                            if (sourceRow == checkOnIndexs[i])
                            {
                                checkOnIndexs[i] = firstIndex;
                                continue;
                            }
                            checkOnIndexs[i]++;
                        }

                        for (int i = 0; i < checkOffIndexs.Length; i++)
                        {
                            checkOffIndexs[i]++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < checkOnIndexs.Length; i++)
                        {
                            if (sourceRow == checkOnIndexs[i])
                            {
                                checkOnIndexs[i] = lastIndex;

                                continue;
                            }
                            checkOnIndexs[i]--;
                        }

                        for (int i = 0; i < checkOffIndexs.Length; i++)
                        {
                            checkOffIndexs[i]--;
                        }
                    }
                }
                else
                {
                    if (mouseUpFlag)
                    {
                        for (int i = 0; i < checkOffIndexs.Length; i++)
                        {
                            if (sourceRow == checkOffIndexs[i])
                            {
                                checkOffIndexs[i] = firstIndex;
                                continue;
                            }
                            checkOffIndexs[i]++;
                        }

                        for (int i = 0; i < checkOnIndexs.Length; i++)
                        {
                            checkOnIndexs[i]++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < checkOffIndexs.Length; i++)
                        {
                            if (sourceRow == checkOffIndexs[i])
                            {
                                checkOffIndexs[i] = lastIndex;
                                continue;
                            }
                            checkOffIndexs[i]--;
                        }

                        for (int i = 0; i < checkOnIndexs.Length; i++)
                        {
                            checkOnIndexs[i]--;
                        }
                    }
                }

                var time = new List<GrapeCity.Win.ElTabelle.Range>();
                foreach (int index in checkOnIndexs)
                {
                    time.Add(new Range(0, index, 0, index));
                }
                GrapeCity.Win.ElTabelle.Range[] checkOnRanges = time.ToArray();

                time = new List<GrapeCity.Win.ElTabelle.Range>();
                foreach (int index in checkOffIndexs)
                {
                    time.Add(new Range(0, index, 0, index));
                }
                GrapeCity.Win.ElTabelle.Range[] checkOffRanges = time.ToArray();

                //画面に反映する
                SetCellRangeValueForCheckBox(checkOnRanges, true);
                SetCellRangeValueForCheckBox(checkOffRanges, false);

                // アクティブセルを変更する
                this.shtMeisai.Focus();
                this.shtMeisai.ActivePosition = objRange.Position;

                // 行入れ替えOFF
                isRowChange = false;

            }
        }
        #endregion

        #region 手配SKS連携更新・削除
        /// --------------------------------------------------
        /// <summary>
        /// 手配SKS連携更新・削除
        /// </summary>
        /// <param name="dtBefore">修正前のデータ</param>
        /// <param name="dtAfter">修正後のデータ</param>
        /// <param name="ds">保存するDataSet</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>J.Chen 2022/10/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckTehaiMeisaiSKS(DataTable dtBefore, DataTable dtAfter, DataSet ds)
        {
            try
            {
                string[] tehaiNoArr1 = null;
                string[] tehaiNoArr2 = null;
                string[] tehaiNoArr3 = null;
                string[] tehaiNoArr4 = null;
                string tehaiRenkeiNo1 = "";
                string tehaiRenkeiNo2 = "";

                DataTable dtUpd = this.GetSchemaTehaiMeisaiSKS("UPDSKS");
                DataTable dtDel = this.GetSchemaTehaiMeisaiSKS("DELSKS");
                DataRow drSKS;

                var conn = new ConnT01();
                var cond = new CondT01(this.UserInfo);


                foreach (DataRow drBefore in dtBefore.Rows)
                {
                    tehaiRenkeiNo1 = ComFunc.GetFld(drBefore, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                    tehaiNoArr1 = ComFunc.GetFld(drBefore, Def_T_TEHAI_SKS.TEHAI_NO).Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (DataRow drAfter in dtAfter.Rows)
                    {
                        tehaiRenkeiNo2 = ComFunc.GetFld(drAfter, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                        tehaiNoArr2 = ComFunc.GetFld(drAfter, Def_T_TEHAI_SKS.TEHAI_NO).Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                        if (tehaiRenkeiNo1 == tehaiRenkeiNo2)
                        {
                            // 削除
                            tehaiNoArr3 = tehaiNoArr1.Except(tehaiNoArr2).ToArray();
                            if (tehaiNoArr3 != null && tehaiNoArr3.Count() > 0)
                            {
                                foreach (string tehaiNo in tehaiNoArr3)
                                {
                                    cond.TehaiNo = tehaiNo;
                                    DataTable dtTehaiSKSWorkVersion = conn.GetTehaiSKSWorkVersion(cond).Tables[Def_T_TEHAI_SKS_WORK.Name];

                                    drSKS = dtDel.NewRow();
                                    drSKS[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO] = tehaiRenkeiNo1;
                                    drSKS[Def_T_TEHAI_SKS.TEHAI_NO] = tehaiNo;
                                    // 検品あり
                                    drSKS[Def_T_TEHAI_SKS.KENPIN_UMU] = KENPIN_UMU.ON_VALUE1;
                                    if (dtTehaiSKSWorkVersion != null && dtTehaiSKSWorkVersion.Rows.Count > 0)
                                    {
                                        UtilData.SetFld(drSKS, Def_T_TEHAI_SKS_WORK.CREATE_DATE, ComFunc.GetFldObject(dtTehaiSKSWorkVersion.Rows[0], Def_T_TEHAI_SKS.CREATE_DATE));
                                    }
                                    dtDel.Rows.Add(drSKS);
                                }
                            }
                            // 更新
                            tehaiNoArr4 = tehaiNoArr2.Except(tehaiNoArr1).ToArray();
                            if (tehaiNoArr4 != null && tehaiNoArr4.Count() > 0)
                            {
                                foreach (string tehaiNo in tehaiNoArr4)
                                {
                                    cond.TehaiNo = tehaiNo;
                                    DataTable dtTehaiSKSWorkVersion = conn.GetTehaiSKSWorkVersion(cond).Tables[Def_T_TEHAI_SKS_WORK.Name];

                                    drSKS = dtUpd.NewRow();
                                    drSKS[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO] = tehaiRenkeiNo1;
                                    drSKS[Def_T_TEHAI_SKS.TEHAI_NO] = tehaiNo;
                                    // 検品あり
                                    drSKS[Def_T_TEHAI_SKS.KENPIN_UMU] = KENPIN_UMU.ON_VALUE1;
                                    if (dtTehaiSKSWorkVersion != null && dtTehaiSKSWorkVersion.Rows.Count > 0)
                                    {
                                        UtilData.SetFld(drSKS, Def_T_TEHAI_SKS_WORK.CREATE_DATE, ComFunc.GetFldObject(dtTehaiSKSWorkVersion.Rows[0], Def_T_TEHAI_SKS.CREATE_DATE));
                                    }
                                    dtUpd.Rows.Add(drSKS);
                                }
                            }
                        }

                    }
                }

                if (dtUpd != null && dtUpd.Rows.Count > 0)
                {
                    ds.Tables.Add(dtUpd);
                }

                if (dtDel != null && dtDel.Rows.Count > 0)
                {
                    ds.Tables.Add(dtDel);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 見積金額影響確認

        /// --------------------------------------------------
        /// <summary>
        /// 見積金額影響確認
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>J.Chen 2023/12/21</create>
        /// <update>J.Chen 2024/03/26 削除行を除外</update>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsEstimatedAmountChanged()
        {
            DataTable dt = this.shtMeisai.DataSource as DataTable;
            string msgAdd = "";
            string estimateNo = "";
            estimateNoList = null;

            if (dt != null && this.tempTable != null)
            {
                // 使用する変数宣言
                int iterationCount = 0;
                int newSize;
                string[] newEstimateNoList;
                int shukkaQtyDt, shukkaQtyTemp, hacchuQtyDt, hacchuQtyTemp, unitPriceDt, unitPriceTemp;

                foreach (DataRow rowDt in dt.Rows)
                {
                    if (rowDt.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }
                    estimateNo = rowDt[Def_T_TEHAI_MEISAI.ESTIMATE_NO].ToString();
                    if (!string.IsNullOrEmpty(estimateNo))
                    {
                        DataRow[] tempRows = this.tempTable.Select(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO + " = '" + rowDt[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO] + "'");

                        if (tempRows.Length > 0)
                        {
                            // 出荷数、発注数と単価を取得
                            shukkaQtyDt = Convert.ToInt32(rowDt.IsNull(Def_T_TEHAI_MEISAI.SHUKKA_QTY) ? 0 : rowDt[Def_T_TEHAI_MEISAI.SHUKKA_QTY]);
                            shukkaQtyTemp = Convert.ToInt32(tempRows[0].IsNull(Def_T_TEHAI_MEISAI.SHUKKA_QTY) ? 0 : tempRows[0][Def_T_TEHAI_MEISAI.SHUKKA_QTY]);
                            hacchuQtyDt = Convert.ToInt32(rowDt.IsNull(Def_T_TEHAI_MEISAI.HACCHU_QTY) ? 0 : rowDt[Def_T_TEHAI_MEISAI.HACCHU_QTY]);
                            hacchuQtyTemp = Convert.ToInt32(tempRows[0].IsNull(Def_T_TEHAI_MEISAI.HACCHU_QTY) ? 0 : tempRows[0][Def_T_TEHAI_MEISAI.HACCHU_QTY]);
                            unitPriceDt = Convert.ToInt32(rowDt.IsNull(Def_T_TEHAI_MEISAI.UNIT_PRICE) ? 0 : rowDt[Def_T_TEHAI_MEISAI.UNIT_PRICE]);
                            unitPriceTemp = Convert.ToInt32(tempRows[0].IsNull(Def_T_TEHAI_MEISAI.UNIT_PRICE) ? 0 : tempRows[0][Def_T_TEHAI_MEISAI.UNIT_PRICE]);

                            // 出荷数もしくは発注数もしくはが異なる場合、変更があると判定
                            if (shukkaQtyDt != shukkaQtyTemp || hacchuQtyDt != hacchuQtyTemp || unitPriceDt != unitPriceTemp)
                            {
                                // すでにestimateNoListに存在していない場合に保存
                                if (estimateNoList == null || !estimateNoList.Contains(estimateNo))
                                {
                                    // 新しいサイズの配列を作成して、既存のデータをコピーし、新しいデータを追加
                                    newSize = (estimateNoList == null) ? 1 : estimateNoList.Length + 1;
                                    newEstimateNoList = new string[newSize];

                                    if (estimateNoList != null)
                                    {
                                        Array.Copy(estimateNoList, newEstimateNoList, estimateNoList.Length);
                                    }

                                    newEstimateNoList[newSize - 1] = estimateNo;

                                    // 新しい配列を元の変数に代入
                                    estimateNoList = newEstimateNoList;
                                }

                                // エラー行取得
                                if (msgAdd.Length == 0)
                                {
                                    msgAdd = (iterationCount + 1).ToString();
                                }
                                else
                                {
                                    msgAdd += "," + (iterationCount + 1).ToString();
                                }
                            }
                        }
                    }
                    iterationCount++;
                }

                if (!string.IsNullOrEmpty(msgAdd))
                {
                    // 見積済みの{0}行目の内容が変更されたため、\r\n受注金額が変更になります。見積の差し替えをお願いいたします。
                    this.ShowMessage("T0100010042", msgAdd);
                    return true;
                }
            }

            return false; // すべての値が同じだった
        }

        #endregion

    }
}