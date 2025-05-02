using System;
using System.Collections.Generic;
using System.Text;
using Commons.Properties;

namespace Commons
{
    /// --------------------------------------------------
    /// <summary>
    /// 定数クラス
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update>T.Nukaga 2019/11/19 STEP12 返却品管理対応</update>
    /// <update>T.Nukaga 2021/12/08 STEP14 enum定義追加</update>
    /// <remarks>
    /// 多言語対応しているので、表示に関する定数はサーバー側で直接使用しないでください。ずっと日本語のままです。
    /// もしサーバーサイドでどうしても使用したい場合は、ConditionやDataSetなどでクライアント側から表示内容を渡すようにしてください。
    /// </remarks>
    /// --------------------------------------------------
    public static class ComDefine
    {
        #region enum

        /// --------------------------------------------------
        /// <summary>
        /// SKS連携時の検索条件処理用
        /// </summary>
        /// <create>T.Nukaga 2021/12/08 STEP14</create>
        /// <update></update>
        /// --------------------------------------------------
        public enum SKSRenkeiZyoken
        {
            /// --------------------------------------------------
            /// <summary>
            /// (SKS)型式が(手配)図番型式と一致
            /// </summary>
            /// <create>T.Nukaga 2021/12/08</create>
            /// <update></update>
            /// --------------------------------------------------
            SKSKatashiki = 0,
            /// --------------------------------------------------
            /// <summary>
            /// (SKS)品番が(手配)図番型式と一致
            /// </summary>
            /// <create>T.Nukaga 2021/12/08</create>
            /// <update></update>
            /// --------------------------------------------------
            SKSHinban = 1,
            /// --------------------------------------------------
            /// <summary>
            /// 品番が図番型式2と一致、または、一致用図番形式(PDM作業名)が一致用図番形式と一致
            /// </summary>
            /// <create>T.Nukaga 2021/12/08</create>
            /// <update></update>
            /// --------------------------------------------------
            SKSHinbanPDM = 2
        }

        #endregion enum

        #region メニュー用データテーブル名

        /// --------------------------------------------------
        /// <summary>
        /// メニュー用データテーブル名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_MENU = "M_MENU";

        #endregion

        #region エラー時のメッセージ

        /// --------------------------------------------------
        /// <summary>
        /// エラー時のメッセージ
        /// </summary>
        /// <create>Y.Higuchi 2010/04/15</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/28 K.Tsutsumi Change カタカナ禁止
        //public static readonly string MSG_ERROR_TEXT = "予期せぬエラーが発生しました。";
        public static string MSG_ERROR_TEXT { get { return Resources.ComDefine_MsgErrorText; } }
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// スプラッシュ時にサーバーに接続できない場合のメッセージ
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/21 K.Tsutsumi Change カタカナ禁止
        //public static readonly string MSG_CONNECTION_ERROR = "サーバーに接続出来ませんでした。\r\nご使用のPC環境またはサーバー環境をご確認ください。";
        public static string MSG_CONNECTION_ERROR { get { return Resources.ComDefine_MsgConnectionError; } }
        // ↑

        /// --------------------------------------------------
        /// <summary>
        /// メッセージが定義されていない場合のメッセージ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/28 K.Tsutsumi Change カタカナ禁止
        //public static readonly string MSG_NOTFOUND_TEXT = "メッセージが定義されていません。";
        public static string MSG_NOTFOUND_TEXT { get { return Resources.ComDefine_MsgNotfoundText; } }
        // ↑

        #endregion

        #region 画面タイトル

        /// --------------------------------------------------
        /// <summary>
        /// パスワード変更
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update>K.Tsutsumi 2011/02/21</update>
        /// --------------------------------------------------
        // 2011/02/21 K.Tsutsumi Change カタカナ禁止
        //public static readonly string TITLE_PASSWORDCHANGE = "パスワード変更";
        public static string TITLE_PASSWORDCHANGE { get { return Resources.ComDefine_TitlePasswordChange; } }
        // ↑

        /// --------------------------------------------------
        /// <summary>
        /// 納入先一覧
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_P0200010 { get { return Resources.ComDefine_DeliverlDestinationList; } }

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別一覧
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_P0200030 { get { return Resources.ComDefine_ConstructionIdentificationList; } }

        /// --------------------------------------------------
        /// <summary>
        /// 物件名一覧
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_P0200040 { get { return Resources.ComDefine_PropertyNameList; } }

        /// --------------------------------------------------
        /// <summary>
        /// 履歴照会
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_P0200050 { get { return Resources.ComDefine_HistoryQuery; } }

        /// --------------------------------------------------
        /// <summary>
        /// 出荷情報明細
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_S0200020 { get { return Resources.ComDefine_ShippingInformationItem; } }

        /// --------------------------------------------------
        /// <summary>
        /// AR情報Help
        /// </summary>
        /// <create>Y.Higuchi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_A0100030 { get { return Resources.ComDefine_ARInformationHelp; } }

        /// --------------------------------------------------
        /// <summary>
        /// AR情報関連付け選択
        /// </summary>
        /// <create>D.Okumura 2020/01/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_A0100031 { get { return Resources.ComDefine_ARInformationRelationSelect; } }

        /// --------------------------------------------------
        /// <summary>
        /// 社内木枠梱包登録
        /// </summary>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_K0200050 { get { return Resources.ComDefine_PalletDetailRegistration; } }

        /// --------------------------------------------------
        /// <summary>
        /// 社外木枠梱包登録
        /// </summary>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_K0200070 { get { return Resources.ComDefine_TagDetailRegistration; } }

        /// --------------------------------------------------
        /// <summary>
        /// 取込エラー詳細(集荷)
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update>K.Tsutsumi 2011/02/21</update>
        /// --------------------------------------------------
        // 2011/02/21 K.Tsutsumi Change カタカナ禁止
        //public static readonly string TITLE_K0400020 = "集荷 - 取込エラー詳細";
        public static string TITLE_K0400020 { get { return Resources.ComDefine_PickupAcquisitionError; } }
        // ↑

        /// --------------------------------------------------
        /// <summary>
        /// 取込エラー詳細(Box梱包)
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update>K.Tsutsumi 2011/02/21</update>
        /// --------------------------------------------------
        // 2011/02/21 K.Tsutsumi Change カタカナ禁止
        //public static readonly string TITLE_K0400030 = "Box梱包 - 取込エラー詳細";
        public static string TITLE_K0400030 { get { return Resources.ComDefine_BoxPackingAcquisitionError; } }
        // ↑

        /// --------------------------------------------------
        /// <summary>
        /// 取込エラー詳細(パレット梱包)
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update>K.Tsutsumi 2011/02/21</update>
        /// --------------------------------------------------
        // 2011/02/21 K.Tsutsumi Change カタカナ禁止
        //public static readonly string TITLE_K0400040 = "パレット梱包 - 取込エラー詳細";
        public static string TITLE_K0400040 { get { return Resources.ComDefine_PalletPackingAcquisitionError; } }
        // ↑

        /// --------------------------------------------------
        /// <summary>
        /// 取込エラー詳細(検品)
        /// </summary>
        /// <create>H.Tajimi 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_K0400050 { get { return Resources.ComDefine_KenpinAcquisitionError; } }
        /// --------------------------------------------------
        /// <summary>
        /// 取込エラー詳細(計測)
        /// </summary>
        /// <create>H.Tajimi 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_K0400060 { get { return Resources.ComDefine_MeasureAcquisitionError; } }

        /// --------------------------------------------------
        /// <summary>
        /// 取込状況
        /// </summary>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_K0400011 { get { return Resources.ComDefine_AcquisitionSituation; } }

        /// --------------------------------------------------
        /// <summary>
        /// AR情報登録
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_A0100020 { get { return Resources.ComDefine_ARInformationRegistration; } }

        /// --------------------------------------------------
        /// <summary>
        /// 号機一覧
        /// </summary>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_A0100022 { get { return Resources.ComDefine_ARJohoMeisaiGokiIchiran; } }

        /// --------------------------------------------------
        /// <summary>
        /// AR情報員数表取込
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_A0100040 { get { return Resources.ComDefine_ARInformationInzuhyoImport; } }

        /// --------------------------------------------------
        /// <summary>
        /// AR進捗管理
        /// </summary>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_A0100050 { get { return Resources.ComDefine_ARShinchokuKanri; } }

        /// --------------------------------------------------
        /// <summary>
        /// 機種ダイアログ
        /// </summary>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_A0100052 { get { return Resources.ComDefine_ShinchokuKishuIchiran; } }

        /// --------------------------------------------------
        /// <summary>
        /// 号機ダイアログ
        /// </summary>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_A0100053 { get { return Resources.ComDefine_ShinchokuGokiIchiran; } }

        /// --------------------------------------------------
        /// <summary>
        /// 変更履歴
        /// </summary>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_A0100060 { get { return Resources.ComDefine_ShinchokuKanriHenkoRireki; } }

        /// --------------------------------------------------
        /// <summary>
        /// 受入情報詳細
        /// </summary>
        /// <create>H.Tsunamura 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_U0100020 { get { return Resources.ComDefine_ReceivingInformartionDetail; } }

        /// --------------------------------------------------
        /// <summary>
        /// 取込状況
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_I0200011 { get { return Resources.ComDefine_AcquisitionSituation; } }

        /// --------------------------------------------------
        /// <summary>
        /// 取込エラー詳細(Location/完了)
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_I0200020 { get { return Resources.ComDefine_LocationAcquisitionError; } }

        /// --------------------------------------------------
        /// <summary>
        /// 取込エラー詳細(棚卸)
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_I0200030 { get { return Resources.ComDefine_StockTakingAcquisitionError; } }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷C/NO入力
        /// </summary>
        /// <create>H.Tajimi 2015/11/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_K0300030 { get { return Resources.ComDefine_PrintCNoInput; } }

        /// --------------------------------------------------
        /// <summary>
        /// 木枠の便間移動
        /// </summary>
        /// <create>T.Wakamatsu 2016/03/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_S0100040 { get { return Resources.ComDefine_FrameShipThroughTransfer; } }

        /// --------------------------------------------------
        /// <summary>
        /// 送信先設定
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_P0200060 { get { return Resources.ComDefine_DestinationSetting; } }

        /// --------------------------------------------------
        /// <summary>
        /// 共通通知設定
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_M0100090 { get { return Resources.ComDefine_CommonInfomationSetting; } }
        /// --------------------------------------------------
        /// <summary>
        /// AR List単位通知設定
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_M0100100 { get { return Resources.ComDefine_ARListUnitInfomationSetting; } }
        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理Mail設定
        /// </summary>
        /// <create>Y.Nakasato 2019/07/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_M0100180 { get { return Resources.ComDefine_SinchokuKanriMailSetting; } }
        /// --------------------------------------------------
        /// <summary>
        /// 計画取込Mail通知先設定
        /// </summary>
        /// <create>Y.Gwon 2023/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_M0100200 { get { return Resources.ComDefine_CommonInfomationSetting1; } }
        /// --------------------------------------------------
        /// <summary>
        /// TAG 入力・変更
        /// </summary>
        /// <create>T.Nakata 2018/11/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_S0100020 { get { return Resources.ComDefine_ShukkaKeikakuMeisai; } }
        /// --------------------------------------------------
        /// <summary>
        /// TAG 発行・照会
        /// </summary>
        /// <create>J.Chen 2023/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_K0100010 { get { return Resources.ComDefine_ShukkaKaishi; } }

        /// --------------------------------------------------
        /// <summary>
        /// 明細分割(出荷明細)
        /// </summary>
        /// <create>T.Nakata 2018/11/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_S0100022 { get { return Resources.ComDefine_ShukkaKeikakuBunkatsu; } }

        /// --------------------------------------------------
        /// <summary>
        /// TAG登録
        /// </summary>
        /// <create>N.Kawamura 2022/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_S0100023 { get { return Resources.ComDefine_TagTouroku; } }

        /// --------------------------------------------------
        /// <summary>
        /// 明細分割(手配明細)
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_T0100011 { get { return Resources.ComDefine_TehaiMeisaiBunkatsu; } }

        /// --------------------------------------------------
        /// <summary>
        /// 手配リスト取込(手配明細)
        /// </summary>
        /// <create>D.Naito 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_T0100012 { get { return Resources.ComDefine_TehaiMeisaiListImport; } }

        /// --------------------------------------------------
        /// <summary>
        /// SKS連携状況確認
        /// </summary>
        /// <create>N.Kawamura 2022/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_T0100020 { get { return Resources.ComDefine_SKSRenkeiJoukyouKakunin; } }

        /// --------------------------------------------------
        /// <summary>
        /// 手配見積
        /// </summary>
        /// <create>S.Furugo 2018/12/4</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_T0100040 { get { return Resources.ComDefine_TehaiEstimate; } }

        /// --------------------------------------------------
        /// <summary>
        /// 手配見積明細
        /// </summary>
        /// <create>S.Furugo 2018/12/4</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_T0100050 { get { return Resources.ComDefine_TehaiEstimateMeisai; } }

        /// --------------------------------------------------
        /// <summary>
        /// 入荷検品登録
        /// </summary>
        /// <create>N.Kawamura 2022/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_T0100060 { get { return Resources.ComDefine_NyuukaKenpinTouroku; } }

        /// --------------------------------------------------
        /// <summary>
        /// 手配情報登録
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_T0100010 { get { return Resources.ComDefine_TehaiMeisai; } }
        /// --------------------------------------------------
        /// <summary>
        /// Ship照会
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_T0100080 { get { return Resources.ComDefine_SearchShipmentNumber; } }
        /// --------------------------------------------------
        /// <summary>
        /// パーツ名翻訳取込
        /// </summary>
        /// <create>H.Tajimi 2019/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_M0100131 { get { return Resources.ComDefine_PartsMeiImport; } }
        /// --------------------------------------------------
        /// <summary>
        /// MAIL送信履歴
        /// </summary>
        /// <create>Y.Gwon 2023/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_S0100060 { get { return Resources.ComDefine_MailSousinRireki; } }
        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画取込
        /// </summary>
        /// <create>J.Chen 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_S0100010 { get { return Resources.ComDefine_ShukkaKeikaku; } }
        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画照会
        /// </summary>
        /// <create>J.Chen 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_S0100070 { get { return Resources.ComDefine_ShukkaKeikakuShuokai; } }

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細履歴
        /// </summary>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string TITLE_T0100100 { get { return Resources.ComDefine_TehaiMeisaiRireki; } }

        #endregion

        #region メニューで使用する画像の一覧

        /// --------------------------------------------------
        /// <summary>
        /// 入力フォーム画像名
        /// </summary>
        /// <create>Y.Higuchi 2010/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string MENU_IMAGE_FORM { get { return Resources.ComDefine_MenuImageForm; } }
        /// --------------------------------------------------
        /// <summary>
        /// 照会フォーム画像名
        /// </summary>
        /// <create>Y.Higuchi 2010/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string MENU_IMAGE_SEARCH_FORM { get { return Resources.ComDefine_MenuImageSearchForm; } }
        /// --------------------------------------------------
        /// <summary>
        /// マスタメンテナンス画像名
        /// </summary>
        /// <create>Y.Higuchi 2010/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string MENU_IMAGE_MASTER { get { return Resources.ComDefine_MenuImageMaster; } }
        /// --------------------------------------------------
        /// <summary>
        /// 検索ダイアログ画像名
        /// </summary>
        /// <create>Y.Higuchi 2010/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string MENU_IMAGE_SEARCH { get { return Resources.ComDefine_MenuImageSearch; } }
        /// --------------------------------------------------
        /// <summary>
        /// 印刷フォーム画像名
        /// </summary>
        /// <create>Y.Higuchi 2010/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string MENU_IMAGE_PRINTER { get { return Resources.ComDefine_MenuImagePrinter; } }
        /// --------------------------------------------------
        /// <summary>
        /// 印刷設定フォーム画像名
        /// </summary>
        /// <create>Y.Higuchi 2010/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string MENU_IMAGE_PRINTER_SETTING { get { return Resources.ComDefine_MenuImagePrinterSetting; } }
        /// --------------------------------------------------
        /// <summary>
        /// その他画像名
        /// </summary>
        /// <create>Y.Higuchi 2010/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string MENU_IMAGE_BATCH { get { return Resources.ComDefine_MenuImageBatch; } }

        #endregion

        #region 集荷開始(K01)で使用するテーブル・フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 現品TAG印刷用テーブル名
        /// </summary>
        /// <create>H.Tsunamura 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_GENPIN = "GENPINTAG";

        /// --------------------------------------------------
        /// <summary>
        /// TAGリスト印刷用テーブル名
        /// </summary>
        /// <create>H.Tsunamura 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_TAGLIST = "TAGLIST";

        /// --------------------------------------------------
        /// <summary>
        /// 発行日付UPDATE用TAGNOリストのテーブル名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_LIST = "LIST";

        /// --------------------------------------------------
        /// <summary>
        /// 印刷時、追加設定用のテーブル名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_CONF = "conf";

        /// --------------------------------------------------
        /// <summary>
        /// 印刷チェックボックスのフィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_INSATU = "INSATU";

        /// --------------------------------------------------
        /// <summary>
        /// バーコード用フィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_BERCODE = "BERCODE";

        /// --------------------------------------------------
        /// <summary>
        /// QRCode用フィールド名
        /// </summary>
        /// <create>D.Okumura 2020/10/26 EFA_SMS-150 QRコードのSHIP欄にARNoを出力する対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_QRCODE = "QRCODE";

        /// --------------------------------------------------
        /// <summary>
        /// 印字位置のフィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_POS = "pos";

        #endregion

        #region 進捗件数取得に使用するテーブル名とフィールド名

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_PROGRESS = "PROGRESS";

        /// --------------------------------------------------
        /// <summary>
        /// 全てのレコードを数えたフィールド名
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_COUNT_ALL = "COUNT_ALL";

        /// --------------------------------------------------
        /// <summary>
        /// 引渡/集荷済のレコードを数えたフィールド名
        /// </summary>
        /// <create>J.Chen 2024/10/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_COUNT_HIKIWATASHI_NO_SHUKA = "COUNT_HIKIWATASHI_NO_SHUKA";

        /// --------------------------------------------------
        /// <summary>
        /// 引渡済のレコードを数えたフィールド名
        /// </summary>
        /// <create>R.Kubota 2023/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_COUNT_HIKIWATASHI = "COUNT_HIKIWATASHI";

        /// --------------------------------------------------
        /// <summary>
        /// 集荷済のレコードを数えたフィールド名
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_COUNT_SHUKA = "COUNT_SHUKA";

        /// --------------------------------------------------
        /// <summary>
        /// BOX梱包済のレコードを数えたフィールド名
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_COUNT_BOXKONPO = "COUNT_BOXKONPO";

        /// --------------------------------------------------
        /// <summary>
        /// Pallet梱包済のレコードを数えたフィールド名
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_COUNT_PALLETKONPO = "COUNT_PALLETKONPO";

        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包済のレコードを数えたフィールド名（※出荷明細情報のみで使用される）
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_COUNT_KIWAKUKONPO = "COUNT_KIWAKUKONPO";

        #endregion

        #region ShippingDocument帳票出力用テーブル/フィールド

        #region 帳票テーブル(テーブルA)

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_SIPPING_A = "SIPPING_A";

        /// --------------------------------------------------
        /// <summary>
        /// Consigned to
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_CONSIGNED_TO = "consigned_to";

        /// --------------------------------------------------
        /// <summary>
        /// INVOICE NO.
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_INVOICE_NO = "invoice_no";

        /// --------------------------------------------------
        /// <summary>
        /// DATE
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_DATE = "date";

        /// --------------------------------------------------
        /// <summary>
        /// PARTS FOR
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_PARTS_FOR = "parts_for";

        /// --------------------------------------------------
        /// <summary>
        /// REF.
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_REF = "ref";

        /// --------------------------------------------------
        /// <summary>
        /// DELIVERY TO
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_DELIVERY_TO = "delivery_to";

        /// --------------------------------------------------
        /// <summary>
        /// CASE MARK
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_CASEMARK = "case_no";

        /// --------------------------------------------------
        /// <summary>
        /// 発行者
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_CREATOR = "creator";

        /// --------------------------------------------------
        /// <summary>
        /// 貿易条件
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TERMS = "terms";

        /// --------------------------------------------------
        /// <summary>
        /// Shape1
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ATTACHED_SHEET_VISIBLE = "attached_sheet_visible";

        /// --------------------------------------------------
        /// <summary>
        /// Shape2
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ATTACHED_SHEET_HIDDEN = "attached_sheet_hidden";

        /// --------------------------------------------------
        /// <summary>
        /// TOTAL(QTY)
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TOTAL_CARTON = "total_carton";

        /// --------------------------------------------------
        /// <summary>
        /// TOTAL(QTY単位)
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TOTAL_CARTON_NAME = "total_carton_name";

        /// --------------------------------------------------
        /// <summary>
        /// TOTAL(YEN)
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TOTAL = "total";

        /// --------------------------------------------------
        /// <summary>
        /// PO#
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_PO_NO = "po_no";

        /// --------------------------------------------------
        /// <summary>
        /// REF. O/#
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_INTERNAL_PO_NO = "internal_po_no";

        /// --------------------------------------------------
        /// <summary>
        /// タイトル
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TITLE = "title";

        /// --------------------------------------------------
        /// <summary>
        /// 出荷先
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SHIP_TO = "ship_to";

        /// --------------------------------------------------
        /// <summary>
        /// 運賃計算基礎重量
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TOTAL_GRWT = "total_grwt";

        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SHIP_NAME = "ship_name";

        #endregion

        #region 荷受けマスタ(テーブルB)

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_SIPPING_B = "SIPPING_B";

        #endregion

        #region 出荷明細(テーブルC)

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_SIPPING_C = "SIPPING_C";

        #endregion

        #region 出荷明細+手配明細(テーブルD)

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_SIPPING_D = "SIPPING_D";

        /// --------------------------------------------------
        /// <summary>
        /// 金額
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_PRICE = "PRICE";

        #endregion

        #region 荷姿明細+納入先マスタ(テーブルE)

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_SIPPING_E = "SIPPING_E";

        /// --------------------------------------------------
        /// <summary>
        /// 寸法(LxWxH)
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SUNPO = "SUNPO";

        /// --------------------------------------------------
        /// <summary>
        /// 寸法(LxWxH) 1/10
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SUNPO_10 = "SUNPO_10";

        /// --------------------------------------------------
        /// <summary>
        /// M3
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_M3 = "M3";

        /// --------------------------------------------------
        /// <summary>
        /// 乙仲
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_OTUNAKA = "OTUNAKA";

        /// --------------------------------------------------
        /// <summary>
        /// N/W
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_NW = "NW";

        /// --------------------------------------------------
        /// <summary>
        /// G/W
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_GW = "GW";

        /// --------------------------------------------------
        /// <summary>
        /// PL_TYPE(表示用)
        /// </summary>
        /// <create>T.Nakata 2018/12/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_DISP_PL_TYPE = "DISP_PL_TYPE";

        /// --------------------------------------------------
        /// <summary>
        /// ESTIMATE_FLAG(表示用)
        /// </summary>
        /// <create>T.Nakata 2018/12/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_DISP_ESTIMATE_FLAG = "DISP_ESTIMATE_FLAG";

        #endregion

        #region 出荷明細+α(テーブルF)

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_SIPPING_F = "SIPPING_F";

        /// --------------------------------------------------
        /// <summary>
        /// NO
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_NO = "list_no";

        /// --------------------------------------------------
        /// <summary>
        /// DESCRIPTION
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_DESCRIPTION = "list_description";

        /// --------------------------------------------------
        /// <summary>
        /// PARTS NO.
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_PARTS = "list_parts";

        /// --------------------------------------------------
        /// <summary>
        /// PARTS NO.2
        /// </summary>
        /// <create>H.Iimuro 2022/10/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_PARTS2 = "list_parts2";

        /// --------------------------------------------------
        /// <summary>
        /// AR NO.
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_AR_NO = "list_ar_no";

        /// --------------------------------------------------
        /// <summary>
        /// CASE NO.
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_CASE_NO = "list_case_no";

        /// --------------------------------------------------
        /// <summary>
        /// QTY
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_QTY = "list_qty";

        /// --------------------------------------------------
        /// <summary>
        /// QTY NAME
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_QTY_NAME = "list_qty_name";

        /// --------------------------------------------------
        /// <summary>
        /// PO NO 
        /// </summary>
        /// <create>H.Kawasaki 2020/01/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_PO_NO = "list_po_no";

        /// --------------------------------------------------
        /// <summary>
        /// UNIT PRICE
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_UNIT_PRICE = "list_unit_price";

        /// --------------------------------------------------
        /// <summary>
        /// TOTAL
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_TOTAL = "list_total";

        /// --------------------------------------------------
        /// <summary>
        /// N/W
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_NET_WEIGHT = "list_net_weight";

        /// --------------------------------------------------
        /// <summary>
        /// MADE IN
        /// </summary>
        /// <create>M.Shimizu 2020/08/25 ShippingDocumentのExcel改修</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_MADE_IN = "list_made_in";

        #endregion

        #region 運送会社マスタ(テーブルG)

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_SIPPING_G = "SIPPING_G";

        #endregion

        #region 出荷明細(一覧)(テーブルH)

        /// --------------------------------------------------
        /// <summary>
        /// テーブル名
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_SIPPING_H = "SIPPING_H";

        #endregion

        #region PO一覧(テーブルI)

        /// --------------------------------------------------
        /// <summary>
        /// PO一覧
        /// </summary>
        /// <create>H.Kawasaki 2021/01/15 </create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_SIPPING_I = "SIPPING_I";

        #endregion

        #region データ収集用DB項目名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細関連情報取得テーブル
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TABLE_PACKKING_SHUKKAMEISAI = "T_PACKING_SHUKKAMEISAI";

        /// --------------------------------------------------
        /// <summary>
        /// TOTAL(YEN)算出フィールド
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TOTALYEN = "PRICE";

        /// --------------------------------------------------
        /// <summary>
        /// 現地通貨フィールド
        /// </summary>
        /// <create>D.Okumura 2020/10/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_PLC = "PLC";

        /// --------------------------------------------------
        /// <summary>
        /// PO# 金額フィールド
        /// </summary>
        /// <create>H.Kawasaki 2021/01/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_PO_TOTAL = "list_po_total";

        #endregion

        #region 帳票項目名

        /// --------------------------------------------------
        /// <summary>
        /// TEL
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_SIPPING_TEL = "TEL:";

        /// --------------------------------------------------
        /// <summary>
        /// FAX
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_SIPPING_FAX = "FAX:";

        /// --------------------------------------------------
        /// <summary>
        /// ATTN
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_SIPPING_ATTN = "ATTN:";

        /// --------------------------------------------------
        /// <summary>
        /// CASEMARK
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_SIPPING_CASEMARK = "C/No";

        /// --------------------------------------------------
        /// <summary>
        /// TOTAL(QTY単位):BOXのみ
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_SIPPING_TOTAL_B = "Carton";

        /// --------------------------------------------------
        /// <summary>
        /// TOTAL(QTY単位):パレットのみ
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update>K.Tsutsumi 2020/11/11 英語に修正</update>
        /// --------------------------------------------------
        public static readonly string REPORT_SIPPING_TOTAL_P = "Pallet";

        /// --------------------------------------------------
        /// <summary>
        /// TOTAL(QTY単位):木枠のみ
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update>K.Tsutsumi 2020/11/11 法律対応</update>
        /// --------------------------------------------------
        public static readonly string REPORT_SIPPING_TOTAL_W = "Wooden Case with IPPC";

        /// --------------------------------------------------
        /// <summary>
        /// TOTAL(QTY単位):混在
        /// </summary>
        /// <create>T.Nakata 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_SIPPING_TOTAL_C = "Case";

        #endregion

        #endregion

        #region 帳票表示に使用する文字列分解フィールド名
        /// --------------------------------------------------
        /// <summary>
        /// 納入先名の0-25バイト分のフィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_NONYUSAKI_NAME_1 = "NONYUSAKI_NAME_1";

        /// --------------------------------------------------
        /// <summary>
        /// 納入先名の25バイト以降のフィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_NONYUSAKI_NAME_2 = "NONYUSAKI_NAME_2";
        /// --------------------------------------------------
        /// <summary>
        /// AREAの0-6バイト分のフィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_AREA_1 = "AREA_1";

        /// --------------------------------------------------
        /// <summary>
        /// AREAの6バイト以降のフィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_AREA_2 = "AREA_2";
        /// --------------------------------------------------
        /// <summary>
        /// FLOORの0-6バイト分のフィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_FLOOR_1 = "FLOOR_1";

        /// --------------------------------------------------
        /// <summary>
        /// FLOORの6バイト以降のフィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_FLOOR_2 = "FLOOR_2";

        /// --------------------------------------------------
        /// <summary>
        /// M-NOの0-8バイト分のフィールド名
        /// </summary>
        /// <create>K.Tsutsumi 2020/10/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_M_NO_1 = "M_NO_1";

        /// --------------------------------------------------
        /// <summary>
        /// M-NOの9バイト分のフィールド名
        /// </summary>
        /// <create>K.Tsutsumi 2020/10/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_M_NO_2 = "M_NO_2";

        /// --------------------------------------------------
        /// <summary>
        /// KISHUの0-16バイト分のフィールド名
        /// </summary>
        /// <create>T.Nukaga 2021/04/07 機種拡張対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_KISHU_1 = "KISHU_1";

        /// --------------------------------------------------
        /// <summary>
        /// KISHUの16バイト以降のフィールド名
        /// </summary>
        /// <create>T.Nukaga 2021/04/07 機種拡張対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_KISHU_2 = "KISHU_2";

        /// --------------------------------------------------
        /// <summary>
        /// 図番/形式の15バイト分のフィールド名
        /// </summary>
        /// <create>T.Nukaga 2021/04/15 機種拡張対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ZUMEN_KEISHIKI_1 = "ZUMEN_KEISHIKI_1";

        /// --------------------------------------------------
        /// <summary>
        /// 図番/形式の15バイト以降のフィールド名
        /// </summary>
        /// <create>T.Nukaga 2021/04/15 機種拡張対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ZUMEN_KEISHIKI_2 = "ZUMEN_KEISHIKI_2";

        /// --------------------------------------------------
        /// <summary>
        /// 手配No（一つ目）
        /// </summary>
        /// <create>J.Chen 2023/02/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAINO_1 = "TEHAINO_1";

        /// --------------------------------------------------
        /// <summary>
        /// 手配No（二つ目）
        /// </summary>
        /// <create>J.Chen 2023/02/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAINO_2 = "TEHAINO_2";

        #endregion

        #region 汎用マスタで定義している名称を表示するための別名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分の名称の別名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SHUKKA_FLAG_NAME = "SHUKKA_FLAG_NAME";

        /// --------------------------------------------------
        /// <summary>
        /// 管理区分の名称の別名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_KANRI_FLAG_NAME = "KANRI_FLAG_NAME";

        /// --------------------------------------------------
        /// <summary>
        /// 手配区分の名称の別名
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAI_FLAG_NAME = "TEHAI_FLAG_NAME";

        /// --------------------------------------------------
        /// <summary>
        /// 入荷状況の名称の別名
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAI_NYUKA_FLAG_NAME = "TEHAI_NYUKA_FLAG_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 組立状況の名称の別名
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAI_ASSY_FLAG_NAME = "TEHAI_ASSY_FLAG_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// TAG登録状況の名称の別名
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAI_TAG_TOUROKU_FLAG_NAME = "TEHAI_TAG_TOUROKU_FLAG_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷状況の名称の別名
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAI_SYUKKA_FLAG_NAME = "TEHAI_SYUKKA_FLAG_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 納入状況の名称の別名
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_NONYU_JYOTAI = "NONYU_JYOTAI";
        /// --------------------------------------------------
        /// <summary>
        /// TAG登録数の別名
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TAG_TOROKU_QTY = "TAG_TOROKU_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷実績数の別名
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SHUKKA_JISSEKI_QTY = "SHUKKA_JISSEKI_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 操作区分
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_HANDY_OPERATION_FLAG_NAME = "HANDY_OPERATION_FLAG_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 状態
        /// </summary>
        /// <create>K.Tsutsumi 2019/09/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_JYOTAI_NAME = "JYOTAI_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 返却品
        /// </summary>
        /// <create>T.Nukaga 2019/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_HENKYAKUHIN_FLAG_NAME = "HENKYAKUHIN_FLAG_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 結果の別名
        /// </summary>
        /// <create>H.Tsuji 2020/06/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_RESULT_STRING = "RESULT_STRING";
        /// <summary>
        /// SKS Skipの別名
        /// </summary>
        /// <create>N.Ikari 2022/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SKS_Skip = "SKS Skip";

        #endregion

        #region 各Noの接頭辞

        /// --------------------------------------------------
        /// <summary>
        /// BoxNoの接頭辞
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PREFIX_BOXNO = "B";

        /// --------------------------------------------------
        /// <summary>
        /// パレットNoの接頭辞
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PREFIX_PALLETNO = "P";

        /// --------------------------------------------------
        /// <summary>
        /// AR No.の接頭辞
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PREFIX_ARNO = "AR";

        #endregion

        #region ログイン情報コピー用

        /// --------------------------------------------------
        /// <summary>
        /// コンディションクラスの接頭辞
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string CONDITION_CLASSNAME_STARTWITH = "Cond";
        /// --------------------------------------------------
        /// <summary>
        /// ログイン情報のプロパティ名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOGININFO_PROPERTY_NAME = "LoginInfo";
        /// --------------------------------------------------
        /// <summary>
        /// ログイン情報の型名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LOGININFO_TYPENAME = "LoginInfo";

        #endregion

        #region 常駐処理

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理のログ出力パス
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update>D.Okumura 2020/05/27 Step13保存ドライブをDドライブへ変更</update>
        /// --------------------------------------------------
        public static readonly string LOG_PUT_PATH = @"E:\ShippingManagementSystem\Log\";
        /// --------------------------------------------------
        /// <summary>
        /// ログのサイクル日数
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int LOG_LIFE_CYCLE = 90;
        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理のカテゴリID
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RESIDENT_CATEGORY_ID = "J01";
        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理のメニュー種別ID
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RESIDENT_MENUITEM_ID = "J0100000";
        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理のタイトル
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string RESIDENT_TITLE { get { return Resources.ComDefine_ResidentTitle; } }
        /// --------------------------------------------------
        /// <summary>
        /// 常駐で使用するLANG ID
        /// ※常駐は日本語固定
        /// </summary>
        /// <create>H.Tajimi 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RESIDENT_LANG = "JP";

        #endregion

        #region 帳票関係

        /// --------------------------------------------------
        /// <summary>
        /// 現品TAGクラス名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100010_CLASS_NAME = "SMS.R01.RepGenpinTag";
        /// --------------------------------------------------
        /// <summary>
        /// 現品TAGドキュメント名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100010_DOCUMENT { get { return Resources.ComDefine_MerchandiseTag; } }
        /// --------------------------------------------------
        /// <summary>
        /// TAGリストのクラス名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100020_CLASS_NAME = "SMS.R01.RepTagList";
        /// --------------------------------------------------
        /// <summary>
        /// TAGリストのドキュメント名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100020_DOCUMENT { get { return Resources.ComDefine_TagList; } }
        /// --------------------------------------------------
        /// <summary>
        /// BOXラベルのクラス名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100030_CLASS_NAME = "SMS.R01.RepBoxLabel";
        /// --------------------------------------------------
        /// <summary>
        /// BOXラベルのドキュメント名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100030_DOCUMENT { get { return Resources.ComDefine_BoxLabel; } }
        /// --------------------------------------------------
        /// <summary>
        /// パレットラベルのクラス名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100040_CLASS_NAME = "SMS.R01.RepPalletLabel";
        /// --------------------------------------------------
        /// <summary>
        /// パレットラベルのドキュメント名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100040_DOCUMENT { get { return Resources.ComDefine_PalletLabel; } }
        /// --------------------------------------------------
        /// <summary>
        /// BOXタグリストのクラス名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100050_CLASS_NAME = "SMS.R01.RepBoxTagList";
        /// --------------------------------------------------
        /// <summary>
        /// BOXタグリストのドキュメント名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100050_DOCUMENT { get { return Resources.ComDefine_BoxTagList; } }
        /// --------------------------------------------------
        /// <summary>
        /// BOXリストのクラス名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100060_CLASS_NAME = "SMS.R01.RepBoxList";
        /// --------------------------------------------------
        /// <summary>
        /// BOXリストのドキュメント名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100060_DOCUMENT { get { return Resources.ComDefine_BoxList; } }
        /// --------------------------------------------------
        /// <summary>
        /// パレットリストのクラス名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100070_CLASS_NAME = "SMS.R01.RepPalletList";
        /// --------------------------------------------------
        /// <summary>
        /// パレットリストのドキュメント名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100070_DOCUMENT { get { return Resources.ComDefine_PalletList; } }
        /// --------------------------------------------------
        /// <summary>
        /// 梱包明細書のクラス名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100080_CLASS_NAME = "SMS.R01.RepKonpoMeisaisho";
        /// --------------------------------------------------
        /// <summary>
        /// 梱包明細書のドキュメント名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100080_DOCUMENT { get { return Resources.ComDefine_PackingDetail; } }
        /// --------------------------------------------------
        /// <summary>
        /// マスタパッキングリストのクラス名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100090_CLASS_NAME = "SMS.R01.RepMasterPackingList";
        /// --------------------------------------------------
        /// <summary>
        /// マスタパッキングリストのドキュメント名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100090_DOCUMENT { get { return Resources.ComDefine_MasterPackingList; } }
        /// --------------------------------------------------
        /// <summary>
        /// パッキングリストのクラス名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100100_CLASS_NAME = "SMS.R01.RepPackingList";
        /// --------------------------------------------------
        /// <summary>
        /// パッキングリストのドキュメント名
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100100_DOCUMENT { get { return Resources.ComDefine_PackingList; } }
        /// --------------------------------------------------
        /// <summary>
        /// ケースマーク画像のフィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_CASE_MARK = "CASE_MARK";

        /// --------------------------------------------------
        /// <summary>
        /// パレットタグリストのクラス名
        /// </summary>
        /// <create>H.Tsunamura 2011/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100110_CLASS_NAME = "SMS.R01.RepPalletTagList";
        /// --------------------------------------------------
        /// <summary>
        /// パレットタグリストのドキュメント名
        /// </summary>
        /// <create>H.Tsunamura 2011/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100110_DOCUMENT { get { return Resources.ComDefine_PalletTagList; } }

        #endregion

        #region 設定ファイル関係

        /// --------------------------------------------------
        /// <summary>
        /// 設定ファイル保存先ディレクトリ名
        /// </summary>
        /// <create>Y.Higuchi 2010/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INI_DIRNAME = @"SMS\";
        /// --------------------------------------------------
        /// <summary>
        /// INIファイル名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INI_FILENAME = "LocalSetting.ini";
        /// --------------------------------------------------
        /// <summary>
        /// プリンタセクション
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SEC_PRINTER = "Printer";
        /// --------------------------------------------------
        /// <summary>
        /// 通常使用するプリンタキー
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KEY_PRINTER_NORMAL = "NormalPrinter";
        /// --------------------------------------------------
        /// <summary>
        /// 現品TAGに使用するプリンタキー
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KEY_PRINTER_TAG = "TAGPrinter";
        /// --------------------------------------------------
        /// <summary>
        /// グリッドINIファイル名
        /// </summary>
        /// <create>H.Tsuji 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string INI_GRID_FILENAME = "GridSetting.ini";

        #endregion

        #region Webサーバーデータ保存パス

        /// --------------------------------------------------
        /// <summary>
        /// Webサーバーのデータ保存フォルダのルートパス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update>D.Okumura 2020/05/27 Step13保存ドライブをDドライブへ変更</update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_ROOT = @"E:\SMS\";
        /// --------------------------------------------------
        /// <summary>
        /// 画像データ保存フォルダのルートパス
        /// </summary>
        /// <create>H.Tajimi 2018/11/13</create>
        /// <update>D.Okumura 2020/05/27 Step13保存ドライブをDドライブへ変更</update>
        /// --------------------------------------------------
        public static readonly string PICTURE_DATA_DIR_ROOT = @"E:\SMS\";
        /// --------------------------------------------------
        /// <summary>
        /// WebサーバーのAR Helpファイル保存パス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_AR = WEB_DATA_DIR_ROOT + @"Help\";
        /// --------------------------------------------------
        /// <summary>
        /// 日次フォルダ名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_NAME_DAILY = @"Daily\";
        /// --------------------------------------------------
        /// <summary>
        /// 月次フォルダ名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_NAME_MONTHLY = @"Monthly\";
        /// --------------------------------------------------
        /// <summary>
        /// CASE MARKフォルダ名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_NAME_CASEMARK = @"casemark\";
        /// --------------------------------------------------
        /// <summary>
        /// 技連フォルダ名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_NAME_GIREN = @"giren\";
        /// --------------------------------------------------
        /// <summary>
        /// テンプレートフォルダ名
        /// </summary>
        /// <create>T.Sakiori 2012/04/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_NAME_TEMPLATE = @"Template\";
        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式画像フォルダ名
        /// </summary>
        /// <create>H.Tajimi 2018/11/06</create>
        /// <update>H.Tajimi 2019/08/08</update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_NAME_KATASHIKI_PICTURES = @"KatashikiPictures\";
        /// --------------------------------------------------
        /// <summary>
        /// 添付ファイル名
        /// </summary>
        /// <create>H.Tajimi 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_NAME_ATTACHMENTS = @"Attachments\";
        /// --------------------------------------------------
        /// <summary>
        /// 凡例フォルダ名
        /// </summary>
        /// <create>D.Okumura 2019/12/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_NAME_LEGEND = @"Legend\";
        /// --------------------------------------------------
        /// <summary>
        /// メールテンプレートフォルダ名
        /// </summary>
        /// <create>D.Okumura 2020/05/29 メールテンプレートダウンロード対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string WEB_DATA_DIR_NAME_MAIL_TEMPLATE = @"Mail\";

        #endregion

        #region ARHelp

        /// --------------------------------------------------
        /// <summary>
        /// ローカルで使用するAR Helpフォルダ名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_HELP_DIR_NAME = "Help";
        /// --------------------------------------------------
        /// <summary>
        /// ARListヘルプ画像ファイル(PNG)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_LIST_HELP_FILE_NAME_1 = "Help1.png";
        /// --------------------------------------------------
        /// <summary>
        /// ARListヘルプ画像ファイル(JPG)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_LIST_HELP_FILE_NAME_2 = "Help1.jpg";
        /// --------------------------------------------------
        /// <summary>
        /// AR情報登録画面ヘルプ画像ファイル(PNG)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_TOROKU_HELP_FILE_NAME_1 = "Help2.png";
        /// --------------------------------------------------
        /// <summary>
        /// AR情報登録画面ヘルプ画像ファイル(JPG)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_TOROKU_HELP_FILE_NAME_2 = "Help2.jpg";
        /// --------------------------------------------------
        /// <summary>
        /// ARヘルプファイル削除パターン
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_HELP_FILE_DELETE_PATTERN = "Help?.*";

        #endregion

        #region カウント取得時の別名

        /// --------------------------------------------------
        /// <summary>
        /// カウント取得時の別名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_CNT = "CNT";

        #endregion

        #region 各更新処理時用データテーブル名

        /// --------------------------------------------------
        /// <summary>
        /// 新規データ用データテーブル名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_INSERT = "INSERT";
        /// --------------------------------------------------
        /// <summary>
        /// 更新データ用データテーブル名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_UPDATE = "UPDATE";
        /// --------------------------------------------------
        /// <summary>
        /// 削除データ用データテーブル名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_DELETE = "DELETE";

        #endregion

        #region 複数メッセージ取得用データテーブルの定数

        /// --------------------------------------------------
        /// <summary>
        /// 複数メッセージデータテーブル名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_MULTIMESSAGE = "MULTIMESSAGE";

        /// --------------------------------------------------
        /// <summary>
        /// 複数メッセージ用メッセージマスタの別名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_MULTIRESULT = "MULTIRESULT";

        /// --------------------------------------------------
        /// <summary>
        /// メッセージのパラメーター
        /// </summary>
        /// <create>Y.Higuchi 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_MESSAGE_PARAMETER = "PARAMETER";


        #endregion

        #region Excel出力関係

        #region Excelファイル名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細のExcelファイル名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_SHUKKA_MEISAI { get { return Resources.ComDefine_ExcelFileShukkaMeisai; } }
        /// --------------------------------------------------
        /// <summary>
        /// AR情報のExcelファイル名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_AR { get { return Resources.ComDefine_ExcelFileAR; } }
        /// --------------------------------------------------
        /// <summary>
        /// 木枠明細のExcelファイル名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_KIWAKU { get { return Resources.ComDefine_ExcelFileFrame; } }
        /// --------------------------------------------------
        /// <summary>
        /// PackingListのExcelファイル名
        /// </summary>
        /// <create>T.Sakiori 2012/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_PACKING_LIST { get { return Resources.ComDefine_ExcelFilePackingList; } }
        /// --------------------------------------------------
        /// <summary>
        /// PackingListのテンプレートファイル名
        /// </summary>
        /// <create>T.Sakiori 2012/04/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_TEMPLATE { get { return Resources.ComDefine_ExcelFileTemplate; } }
        /// --------------------------------------------------
        /// <summary>
        /// 在庫リストのExcelファイル名
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_ZAIKO { get { return Resources.ComDefine_ExcelFileZaiko; } }
        /// --------------------------------------------------
        /// <summary>
        /// 棚卸リストのExcelファイル名
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_TANAOROSHI { get { return Resources.ComDefine_ExcelFileTanaoroshi; } }
        /// --------------------------------------------------
        /// <summary>
        /// オペ履歴リストのExcelファイル名
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_OPE_RIREKI { get { return Resources.ComDefine_ExcelFileOperireki; } }
        /// --------------------------------------------------
        /// <summary>
        /// AR対応費用のExcelファイル名
        /// </summary>
        /// <create>H.Tajimi 2018/10/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_AR_COST { get { return Resources.ComDefine_ExcelFileARCost; } }
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿のExcelファイル名
        /// </summary>
        /// <create>H.Tajimi 2018/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_PACKING { get { return Resources.ComDefine_ExcelFilePacking; } }
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿のテンプレートファイル名
        /// </summary>
        /// <create>H.Tajimi 2018/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_PACKING_TEMPLATE { get { return Resources.ComDefine_ExcelFilePackingTemplate; } }
        /// --------------------------------------------------
        /// <summary>
        /// PartsListのテンプレートファイル名
        /// </summary>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_PARTS_LIST_TEMPLATE { get { return Resources.ComDefine_ExcelFilePartsListTemplate; } }
        /// --------------------------------------------------
        /// <summary>
        /// 有償支給部品のExcelファイル名
        /// </summary>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_ESTIMATE_PARTS { get { return Resources.ComDefine_ExcelFileEstimateParts; } }
        /// --------------------------------------------------
        /// <summary>
        /// 有償支給部品のExcelテンプレートファイル名
        /// </summary>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_TEMP_ESTIMATE_PARTS { get { return Resources.ComDefine_ExcelFileTempEstimateParts; } }
        /// --------------------------------------------------
        /// <summary>
        /// INVOICEのExcelテンプレートファイル名
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_TEMP_INVOICE { get { return Resources.ComDefine_ExcelFileTempInvoice; } }
        /// --------------------------------------------------
        /// <summary>
        /// INVOICEのExcelファイル名
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_INVOICE { get { return Resources.ComDefine_ExcelFileInvoice; } }
        /// --------------------------------------------------
        /// <summary>
        /// INVOICE(国内)のExcelファイル名
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_PARTSLISTDETAIL { get { return Resources.ComDefine_ExcelFilePartsListDetail; } }
        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細(Sip)のExcelファイル名
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_SIP_SYUKKAMEISAI { get { return Resources.ComDefine_ExcelFileInvoice; } }
        /// --------------------------------------------------
        /// <summary>
        /// まとめ表パーツ便物量実績のExcelテンプレートファイル名
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_TEMP_QUANTITY_OF_PARTSSHIPMENT { get { return Resources.ComDefine_ExcelFileTempQuantityOfPartsShipment; } }
        /// --------------------------------------------------
        /// <summary>
        /// まとめ表パーツ便物量実績のExcelファイル名
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_QUANTITY_OF_PARTSSHIPMENT { get { return Resources.ComDefine_ExcelFileQuantityOfPartsShipment; } }
        /// --------------------------------------------------
        /// <summary>
        /// まとめ表パーツ便出荷のExcelテンプレートファイル名
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_TEMP_SUMMARY_OF_PARTSSHIPMENT { get { return Resources.ComDefine_ExcelFileTempSummaryOfPartsShipment; } }
        /// --------------------------------------------------
        /// <summary>
        /// まとめ表パーツ便出荷のExcelファイル名
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_SUMMARY_OF_PARTSSHIPMENT { get { return Resources.ComDefine_ExcelFileSummaryOfPartsShipment; } }
        /// --------------------------------------------------
        /// <summary>
        /// 手配明細のExcelファイル名
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_TEHAI_MEISAI { get { return Resources.ComDefine_ExcelFileTehaiMeisai; } }
        /// --------------------------------------------------
        /// <summary>
        /// 担当者のExcelファイル名
        /// </summary>
        /// <create>H.Tajimi 2019/07/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_USER_MASTER { get { return Resources.ComDefine_ExcelFileUserMaster; } }
        /// --------------------------------------------------
        /// <summary>
        /// パーツ名翻訳保守ファイル名
        /// </summary>
        /// <create>H.Tajimi 2019/07/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_PARTS_NAME_HONYAKU { get { return Resources.ComDefine_ExcelFilePartsNameHonyaku; } }
        /// --------------------------------------------------
        /// <summary>
        /// AR進捗管理のExcelファイル名
        /// </summary>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_AR_SHINCHOKU { get { return Resources.ComDefine_ExcelFileArShinchoku; } }
        /// --------------------------------------------------
        /// <summary>
        /// 検品取込ERROR詳細のExcelファイル名
        /// </summary>
        /// <create>H.Tsuji 2020/06/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_HANDYDATAERRORMEISAI_KENPIN { get { return Resources.ComDefine_ExcelFileHandyDataErrorMeisaiKenpin; } }
        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画のテンプレートファイル名
        /// </summary>
        /// <create>T.SASAYAMA 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_SHUKKAKEIKAKU_TEMPLATE { get { return Resources.ComDefine_ExcelFileShukkaKeikakuTemplate; } }
        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画照会のExcelファイル名
        /// </summary>
        /// <create>J.Chen 2023/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_SHUKKA_KEIKAKU_SHOKAI { get { return Resources.ComDefine_ExcelFileShukkaKeikakuShokai; } }
        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画のExcelファイル名
        /// </summary>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_SHUKKA_KEIKAKU { get { return Resources.ComDefine_ExcelFileShukkaKeikaku; } }
        /// --------------------------------------------------
        /// <summary>
        /// Quotationのテンプレートファイル名
        /// </summary>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_QUOTATION_TEMPLATE { get { return Resources.ComDefine_ExcelFileQuotationTemplate; } }
        /// --------------------------------------------------
        /// <summary>
        /// 見積書メール送信用ファイル名
        /// </summary>
        /// <create>J.Chen 2024/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_QUOTATION { get { return Resources.ComDefine_ExcelFileQuotation; } }
        /// --------------------------------------------------
        /// <summary>
        /// 見積書メール送信用zipファイル名
        /// </summary>
        /// <create>J.Chen 2024/01/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_QUOTATION_FOR_ZIP { get { return Resources.ComDefine_ExcelFileQuotationForZip; } }
        /// --------------------------------------------------
        /// <summary>
        /// 見積明細照会のExcelファイル名
        /// </summary>
        /// <create>J.Chen 2024/02/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_MITSUMORIMEISAI { get { return Resources.ComDefine_ExcelFileMitsumoriMeisai; } }
        /// --------------------------------------------------
        /// <summary>
        /// 配送先のExcelファイル名
        /// </summary>
        /// <create>J.Chen 2024/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_DELIVER { get { return Resources.ComDefine_ExcelFileHaisosakiHoshu; } }
        /// --------------------------------------------------
        /// <summary>
        /// 配送先のExcelのテンプレートファイル名
        /// </summary>
        /// <create>J.Chen 2024/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXCEL_FILE_DELIVER_TEMPLATE { get { return Resources.ComDefine_ExcelFileDeliverTemplate; } }

        #endregion

        #endregion

        #region 出荷情報登録用

        /// --------------------------------------------------
        /// <summary>
        /// 画面下部に表示する運送会社等の情報を保持するテーブル
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_ADDITION = "ADDITION";
        /// --------------------------------------------------
        /// <summary>
        /// 状態ボタンの表示文字列を保持する列名(梱包情報保守でも使用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_BTN_STATE = "BUTTON_STATE";
        /// --------------------------------------------------
        /// <summary>
        /// 詳細ボタンの表示文字列を保持する列名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_BTN_DETAIL = "BUTTON_DETAIL";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日のカウント(梱包情報保守でも使用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SHUKKA_CNT = "SHUKKA_CNT";
        /// --------------------------------------------------
        /// <summary>
        /// 受入日のカウント
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_UKEIRE_CNT = "UKEIRE_CNT";
        /// --------------------------------------------------
        /// <summary>
        /// C/NO(SHIP + '-' + CASE_NO)の列名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SHIP_CNO = "CNO";
        /// --------------------------------------------------
        /// <summary>
        /// 表示名(納入先名 or 工事識別名称)の列名(ADDITION用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ADDITION_DISP_NAME = "DISP_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷便の列名(ADDITION用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ADDITION_DISP_SHIP = "DISP_SHIP";
        /// --------------------------------------------------
        /// <summary>
        /// T_AR.ARNo.の列名(ADDITION用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ADDITION_DISP_AR_NO = "DISP_AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// T_SHUKKA_MEISAI.ARNo.の列名(ADDITION用)
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ADDITION_TSM_AR_NO = "TSM_AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 運送会社の列名(ADDITION用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ADDITION_UNSOKAISHA = "UNSOKAISHA_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// インボイスNo.の列名(ADDITION用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ADDITION_INVOICE_NO = "INVOICE_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 送り状No.の列名(ADDITION用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ADDITION_OKURIJYO_NO = "OKURIJYO_NO";
        /// --------------------------------------------------
        /// <summary>
        /// BLNo.の列名(ADDITION用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ADDITION_BL_NO = "BL_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付の列名(ADDITION用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ADDITION_SHUKKA_DATE = "SHUKKA_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 受入日付の列名(ADDITION用)
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ADDITION_UKEIRE_DATE = "UKEIRE_DATE";
        /// --------------------------------------------------
        /// <summary>
        /// 状態ボタンに表示する梱包済
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string BUTTON_TEXT_KONPOZUMI { get { return Resources.ComDefine_ButtonTextKonpozumi; } }
        /// --------------------------------------------------
        /// <summary>
        /// 状態ボタンに表示する出荷
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string BUTTON_TEXT_SHUKKA { get { return Resources.ComDefine_ButtonTextShukka; } }
        /// --------------------------------------------------
        /// <summary>
        /// 詳細ボタンに表示する文字
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string BUTTON_TEXT_DETAIL { get { return Resources.ComDefine_ButtonTextDetail; } }

        #endregion

        #region 共通

        /// --------------------------------------------------
        /// <summary>
        /// 結果テーブル
        /// </summary>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_RESULT = "RESULT";
        /// --------------------------------------------------
        /// <summary>
        /// 結果列
        /// </summary>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_RESULT = "RESULT";
        /// --------------------------------------------------
        /// <summary>
        /// 写真無の表示値
        /// </summary>
        /// <create>H.Tajimi 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string NOT_EXISTS_PICTURE_VALUE = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 写真有の表示値
        /// </summary>
        /// <create>H.Tajimi 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string EXISTS_PICTURE_VALUE { get { return Resources.ComDefine_Exists; } }
        /// --------------------------------------------------
        /// <summary>
        /// SHIP OR ARNo
        /// </summary>
        /// <create>H.Tajimi 2019/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SHIP_AR_NO = "SHIP_AR_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 帳票チェックの表示値
        /// </summary>
        /// <create>H.Tsuji 2020/06/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_CHECK_VALUE { get { return Resources.ComDefine_ReportCheck; } }
        /// --------------------------------------------------
        /// <summary>
        /// 英語(米国)版での言語
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LANG_JP = "JP";
        /// --------------------------------------------------
        /// <summary>
        /// 英語(米国)版での言語
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LANG_US = "US";

        #endregion

        #region 受入登録用

        /// --------------------------------------------------
        /// <summary>
        /// 状態ボタンに表示する受入
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string BUTTON_TEXT_UKEIRE { get { return Resources.ComDefine_ButtonTextUkeire; } }

        #endregion

        #region 出荷計画取込用

        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグの列名(値)
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_EXCEL_SHORI_FLAG = "EXCEL_SHORI_FLAG";
        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグの列名(表示)
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_EXCEL_SHORI_FLAG_NAME = "EXCEL_SHORI_FLAG_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// 有償/無償の列名
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ESTIMATE_FLAG_NAME = "ESTIMATE_FLAG_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// AIR/SHIPの列名
        /// </summary>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TRANSPORT_FLAG_NAME = "TRANSPORT_FLAG_NAME";

        #endregion

        #region 一括アップロード用

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細 図番/型式(全角)
        /// </summary>
        /// <create>H.Tajimi 2019/08/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_MB_ZUMEN_KEISHIKI = "MB_ZUMEN_KEISHIKI";
        /// --------------------------------------------------
        /// <summary>
        /// 写真有無(図番/型式用)
        /// </summary>
        /// <create>H.Tajimi 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_EXISTS_PICTURE = "EXISTS_PICTURE";

        #endregion

        #region 組立登録用

        /// --------------------------------------------------
        /// <summary>
        /// 残数
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_REMAIN_QTY = "REMAIN_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 登録予定の組立数
        /// </summary>
        /// <create>H.Tajimi 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_NEW_ASSY_QTY = "NEW_ASSY_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 行位置
        /// </summary>
        /// <create>H.Tajimi 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ROW_INDEX = "ROW_INDEX";

        #endregion

        #region ラベルの表示を切り替える時に使用する

        /// --------------------------------------------------
        /// <summary>
        /// 詳細ボタンに表示する文字
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string LABEL_CAPTION_NONYUSAKI { get { return Resources.ComDefine_LabelCaptionNonyusaki; } }
        /// --------------------------------------------------
        /// <summary>
        /// 詳細ボタンに表示する文字
        /// </summary>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string LABEL_CAPTION_KOJI_NAME { get { return Resources.ComDefine_LabelCaptionKojiName; } }

        #endregion

        #region シートの編集ボタンの設定

        /// --------------------------------------------------
        /// <summary>
        /// 編集ボタンのフィールド名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_BTN_EDIT = "BUTTON_EDIT";

        #endregion

        #region ハンディデータ取込

        /// --------------------------------------------------
        /// <summary>
        /// 作業文字列のカラム名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TORIKOMI_SAGYO = "TORIKOMI_SAGYO";
        /// --------------------------------------------------
        /// <summary>
        /// 取込データを保存するベースフォルダ名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string IMPORT_TEMP_BASE_DIR = "Handy";
        /// --------------------------------------------------
        /// <summary>
        /// 引渡ファイルの拡張子なしファイル名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_HIKIWATASHI_FILENAME = "HIKIWATASHI";
        /// --------------------------------------------------
        /// <summary>
        /// 集荷ファイルの拡張子なしファイル名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_SHUKA_FILENAME = "SHUKA";
        /// --------------------------------------------------
        /// <summary>
        /// Box梱包のファイル拡張子
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_BOX_FILEEXT = "B";
        /// --------------------------------------------------
        /// <summary>
        /// パレット梱包のファイル拡張子
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_PALLETEXT = "P";
        /// --------------------------------------------------
        /// <summary>
        /// 計測のファイル拡張子
        /// </summary>
        /// <create>H.Tajimi 2018/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_KEISOKUEXT = "M";
        /// --------------------------------------------------
        /// <summary>
        /// 検品のファイル拡張子
        /// </summary>
        /// <create>H.Tajimi 2018/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_KENPINEXT = "R";
        /// --------------------------------------------------
        /// <summary>
        /// 引渡のファイル拡張子
        /// </summary>
        /// <create>H.Tajimi 2018/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_HIKIWATASHIEXT = "W";
        /// --------------------------------------------------
        /// <summary>
        /// エラー行の色
        /// </summary>
        /// <create>J.Chen 2024/11/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ERROR_COLOR = "FFFF00,FF0000";

        #endregion

        #region SQL Serverの一意制約違反のエラー番号

        /// --------------------------------------------------
        /// <summary>
        /// SQL Serverの一意制約違反のエラー番号(プライマリキー)
        /// </summary>
        /// <create>Y.Higuchi 2010/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int DB_DUPLICATION_ERRORNO = 2627;
        /// --------------------------------------------------
        /// <summary>
        /// SQL Serverの一意制約違反のエラー番号(ユニークインデックス)
        /// </summary>
        /// <create>Y.Higuchi 2010/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int DB_DUPLICATION_ERRORNO_UNIQUEINDEX = 2601;

        #endregion

        #region 梱包情報保守

        /// --------------------------------------------------
        /// <summary>
        /// 状態ボタンに表示する梱包
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string BUTTON_TEXT_KONPO { get { return Resources.ComDefine_ButtonTextKonpo; } }
        /// --------------------------------------------------
        /// <summary>
        /// 状態ボタンに表示する解除
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string BUTTON_TEXT_KAIJYO { get { return Resources.ComDefine_ButtonTextKaijyo; } }
        /// --------------------------------------------------
        /// <summary>
        /// 状態ボタンに表示する登録
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string BUTTON_TEXT_TOUROKU { get { return Resources.ComDefine_ButtonTextTouroku; } }
        /// --------------------------------------------------
        /// <summary>
        /// パレットNo.のカウント
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_PALLET_NO_CNT = "PALLET_NO_CNT";
        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NOのカウント
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_KOJI_NO_CNT = "KOJI_NO_CNT";

        #endregion

        #region 締め処理

        /// --------------------------------------------------
        /// <summary>
        /// 締め処理で使用する更新ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHIME_UPDATE_USER_ID = " ";
        /// --------------------------------------------------
        /// <summary>
        /// 締め処理で使用する更新ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string SHIME_UPDATE_USER_NAME { get { return Resources.ComDefine_ShimeUpdateUserName; } }
        /// --------------------------------------------------
        /// <summary>
        /// バックアップ時に使用するDB名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DB_BACKUP_DBNAME = "SMS";
        /// --------------------------------------------------
        /// <summary>
        /// バックアップファイル名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DB_BACKUP_FILE_NAME = "{0}.bak";
        /// --------------------------------------------------
        /// <summary>
        /// バックアップ名称
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string DB_BACKUP_BACKUP_NAME { get { return Resources.ComDefine_DbBackupBackupName; } }


        #endregion

        #region SKS連携処理

        /// --------------------------------------------------
        /// <summary>
        /// 発注状態名
        /// </summary>
        /// <create>H.Tajimi 2018/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_HACCHU_ZYOTAI_NAME = "HACCHU_ZYOTAI_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// SKS連携処理で使用するユーザーID
        /// </summary>
        /// <create>H.Tajimi 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SKS_RENKEI_USER_ID = " ";
        /// --------------------------------------------------
        /// <summary>
        /// SKS連携処理で使用するユーザー名
        /// </summary>
        /// <create>H.Tajimi 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string SKS_RENKEI_USER_NAME { get { return Resources.ComDefine_SKSRenkeiUserName; } }

        #endregion

        #region AR技連ファイル

        /// --------------------------------------------------
        /// <summary>
        /// 技連ファイル最大サイズ
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly long GIREN_FILE_MAX_SIZE = 10 * 1024 * 1024;

        #endregion

        #region AR参考資料ファイル

        /// --------------------------------------------------
        /// <summary>
        /// AR参考資料ファイル最大サイズ
        /// </summary>
        /// <create>H.Tajimi 2018/10/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly long REF_FILE_MAX_SIZE = GIREN_FILE_MAX_SIZE;

        #endregion

        #region AR添付ファイル一時保存フォルダ

        /// --------------------------------------------------
        /// <summary>
        /// AR添付ファイル一時保存フォルダ
        /// </summary>
        /// <create>J.Chen 2024/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string AR_OUTPUT_DIR = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"sms\AR");

        #endregion

        #region ARメール送信用zipファイル名

        /// --------------------------------------------------
        /// <summary>
        /// ARメール送信用zipファイル名
        /// </summary>
        /// <create>J.Chen 2024/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string ATTACHMENT_FILE_AR_FOR_ZIP { get { return Resources.ComDefine_AttachmentFileArForZip; } }

        #endregion

        #region 出荷明細画像ファイル

        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細画像ファイル拡張子
        /// </summary>
        /// <create>H.Tajimi 2018/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string[] SHUKKA_MEISAI_FILE_EXT = new string[] { @".jpg" };

        #endregion

        #region DBオープンリトライ回数

        /// --------------------------------------------------
        /// <summary>
        /// DBオープンリトライ回数
        /// </summary>
        /// <create>Y.Higuchi 2010/10/28</create>
        /// <update>Y.Higuchi 2010/11/18 リトライしても改善しなかったのでSOAP通信自体をリトライするようにしたのでリトライ数を減らす。</update>
        /// --------------------------------------------------
        public static readonly int DB_OPEN_RETRY_COUNT = 1;

        #endregion

        #region ROLEID

        /// --------------------------------------------------
        /// <summary>
        /// 管理者の権限ID(配列) AR情報登録で使用
        /// </summary>
        /// <create>H.Tsunamura 2010/10/28</create>
        /// <update>J.Chen 2023/10/10 生産管理者追加</update>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string[] ROLE_KANRISYA = { "ZZZ", "999", "005", "010" };

        #endregion

        #region 木枠No(出荷計画明細・出荷情報照会)

        /// --------------------------------------------------
        /// <summary>
        /// 木枠No.のフィールド名
        /// </summary>
        /// <create>Y.Higuchi 2010/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_KIWAKU_NO = "KIWAKU_NO";

        #endregion

        #region Downloadフォルダパス

        /// --------------------------------------------------
        /// <summary>
        /// Downloadフォルダパス
        /// </summary>
        /// <create>T.Sakiori 2012/05/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DOWNLOAD_DIR = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"sms\Downloads");

        #endregion


        #region 凡例ダウンロードフォルダ名

        /// --------------------------------------------------
        /// <summary>
        /// 凡例ダウンロードフォルダ名
        /// </summary>
        /// <create>D.Okumura 2019/12/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DOWNLOAD_DIR_LEGEND = "Legend";

        #endregion

        #region Resizeフォルダパス

        /// --------------------------------------------------
        /// <summary>
        /// Resizeフォルダパス
        /// </summary>
        /// <create>K.Tsutsumi 2019/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RESIZE_DIR = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"sms\_resize");

        #endregion

        #region Resize
        /// --------------------------------------------------
        /// <summary>
        /// Resize時の解像度(垂直)
        /// </summary>
        /// <create>K.Tsutsumi 2019/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly Single RESIZE_VERTICAL_RESOLUSION = 72.0F;
        /// --------------------------------------------------
        /// <summary>
        /// Resize時の解像度(水平)
        /// </summary>
        /// <create>K.Tsutsumi 2019/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly Single RESIZE_HORIZONTAL_RESOLUSION = 72.0F;
        /// --------------------------------------------------
        /// <summary>
        /// Resize時の幅
        /// </summary>
        /// <create>K.Tsutsumi 2019/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int RESIZE_WIDTH = 640;
        /// --------------------------------------------------
        /// <summary>
        /// Resize時の高さ
        /// </summary>
        /// <create>K.Tsutsumi 2019/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int RESIZE_HEIGHT = 480;
        /// --------------------------------------------------
        /// <summary>
        /// Resize時のエンコードの種類
        /// </summary>
        /// <create>K.Tsutsumi 2019/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string RESIZE_ENCODE = "image/jpeg";

        #endregion

        #region 在庫管理

        /// --------------------------------------------------
        /// <summary>
        /// 入庫ロケーションのカラム名
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_NYUKO_LOCATION = "NYUKO_LOCATION";

        /// --------------------------------------------------
        /// <summary>
        /// 作業区分名のカラム名
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SAGYO_FLAG_NAME = "SAGYO_FLAG_NAME";

        /// --------------------------------------------------
        /// <summary>
        /// 状態名のカラム名
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_STATUS_NAME = "STATUS_NAME";

        /// --------------------------------------------------
        /// <summary>
        /// TagCode
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TAG_CODE = "TAG_CODE";


        #endregion

        #region ハンディデータ（部品管理用）取込

        /// --------------------------------------------------
        /// <summary>
        /// 取込データを保存するベースフォルダ名
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string IMPORT_BUHIN_TEMP_BASE_DIR = "HandyBuhin";
        /// --------------------------------------------------
        /// <summary>
        /// Location登録保存拡張子
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_LOCATION_FILE_EXT = "LOC";
        /// --------------------------------------------------
        /// <summary>
        /// 完了登録保存拡張子
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_KANRYO_FILE_EXT = "CMP";
        /// --------------------------------------------------
        /// <summary>
        /// 棚卸登録保存拡張子
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HANDY_TANAOROSHI_FILE_EXT = "IVT";

        #endregion

        #region 入力バイト数
        /// --------------------------------------------------
        /// <summary>
        /// 入力バイト数（機種）
        /// </summary>
        /// <create>K.Tsutsumi 2014/03/17</create>
        /// <update>T.Nukaga 2021/04/02 機種桁数拡張16→30</update>
        /// <update>J.Chen 2023/02/15 機種桁数拡張30→40</update>
        /// --------------------------------------------------
        public static readonly int MAX_BYTE_LENGTH_KISHU = 40;
        /// --------------------------------------------------
        /// <summary>
        /// 入力バイト数（号機）
        /// </summary>
        /// <create>K.Tsutsumi 2014/03/17</create>
        /// <update>D.Okumura 2019/08/05 号機桁数拡張20→40</update>
        /// --------------------------------------------------
        public static readonly int MAX_BYTE_LENGTH_GOKI = 40;
        /// --------------------------------------------------
        /// <summary>
        /// 入力バイト数（発生要因）
        /// </summary>
        /// <create>H.Tajimi 2018/10/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int MAX_BYTE_LENGTH_HASSEI_YOUIN = 40;
        /// --------------------------------------------------
        /// <summary>
        /// 入力バイト数（図番/型式の検索用フィールド）
        /// </summary>
        /// <create>K.Tsutsumi 2019/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int MAX_BYTE_LENGTH_ZUMEN_KEISHIKI_S = 30;
        /// --------------------------------------------------
        /// <summary>
        /// 入力バイト数（図番/型式）
        /// </summary>
        /// <create>K.Tsutsumi 2019/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int MAX_BYTE_LENGTH_ZUMEN_KEISHIKI = 30;
        /// --------------------------------------------------
        /// <summary>
        /// 入力バイト数（Customer名）
        /// </summary>
        /// <create>H.Tsuji 2020/06/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int MAX_BYTE_LENGTH_CUSTOMER_NAME = 80;
        /// --------------------------------------------------
        /// <summary>
        /// 入力バイト数（納入場所）
        /// </summary>
        /// <create>H.Tsuji 2020/06/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int MAX_BYTE_LENGTH_NONYUBASHO = 14;
        /// --------------------------------------------------
        /// <summary>
        /// 入力バイト数（注文書品目名称）
        /// </summary>
        /// <create>H.Tsuji 2020/06/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int MAX_BYTE_LENGTH_CHUMONSHO_HINMOKU = 60;

        #endregion

        #region 選択チェックボックスのフィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 選択チェックボックスのフィールド名
        /// </summary>
        /// <create>H.Tajimi 2015/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SELECT_CHK = "SELECT_CHK";

        #endregion

        #region メールアドレス変更権限のフィールド名

        /// --------------------------------------------------
        /// <summary>
        /// メールアドレス変更権限のフィールド名
        /// </summary>
        /// <create>R.Katsuo 2017/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_MAIL_CHANGE_ROLE_NAME = "MAIL_CHANGE_ROLE_NAME";

        #endregion

        #region スタッフ区分のフィールド名

        /// --------------------------------------------------
        /// <summary>
        /// スタッフ区分のフィールド名
        /// </summary>
        /// <create>H.Tajimi 2018/10/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_STAFF_KBN_NAME = "STAFF_KBN_NAME";

        #endregion

        #region 荷姿表送信対象のフィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿表送信対象のフィールド名
        /// </summary>
        /// <create>H.Tajimi 2019/07/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_MAIL_PACKING_FLAG_NAME = "MAIL_PACKING_FLAG_NAME";

        #endregion

        #region TAG連携送信対象のフィールド名

        /// --------------------------------------------------
        /// <summary>
        /// TAG連携送信対象のフィールド名
        /// </summary>
        /// <create>H.Tajimi 2019/08/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_MAIL_TAG_RENKEI_FLAG_NAME = "MAIL_TAG_RENKEI_FLAG_NAME";

        #endregion

        #region TAG連携送信対象のフィールド名

        /// --------------------------------------------------
        /// <summary>
        /// 計画取込一括設定のフィールド名
        /// </summary>
        /// <create>J.Chen 2024/01/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_MAIL_SHUKKAKEIKAKU_FLAG_NAME = "MAIL_SHUKKAKEIKAKU_FLAG_NAME";

        #endregion

        #region コンボボックス関連

        #region コンボボックスで使用する『全て』関連

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスで使用する固定で追加した1行目の値
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string COMBO_FIRST_VALUE  = "__FIRST__";
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスで使用する『全て』の値
        /// </summary>
        /// <create>R.Sumi 2022/02/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string COMBO_ALL_VALUE = "__ALL__";
        /// --------------------------------------------------
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスで使用する『全て(AR)』の値
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string COMBO_ALL_AR_VALUE = "__AR__";
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスで使用する『全て(AR 未出荷)』の値
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public const string COMBO_ALL_MISHUKKA_AR_VALUE = "__MISHUKKA_AR__";
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスで使用する『全て』の表示文字列
        /// </summary>
        /// <create>H.Tajimi 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string COMBO_ALL_DISP { get { return Resources.ComDefine_ComboAllDisp; } }
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスで使用する『全て(AR)』の表示文字列
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string COMBO_ALL_AR_DISP { get { return Resources.ComDefine_ComboAllARDisp; } }
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスで使用する『全て(AR 未出荷)』の表示文字列
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string COMBO_ALL_MISHUKKA_AR_DISP { get { return Resources.ComDefine_ComboAllMishukkaARDisp; } }
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスで使用する『全て』の値（物件名マスタ）
        /// </summary>
        /// <create>J.Chen 2023/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public const decimal COMBO_ALL_VALUE_DECIMAL = 0;

        #endregion

        #endregion

        #region 回答納期

        /// --------------------------------------------------
        /// <summary>
        /// 回答納期なし
        /// </summary>
        /// <create>H.Tajimi 2018/11/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KAITO_DATE_NONE = "0";
        /// --------------------------------------------------
        /// <summary>
        /// 回答納期(完納)
        /// </summary>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string KAITO_DATE_KANNOU = "11111111";

        #endregion

        #region TAG連携処理

        #region TAG入力・変更画面カテゴリID

        /// --------------------------------------------------
        /// <summary>
        /// TAG入力・変更画面カテゴリID
        /// </summary>
        /// <create>T.Nakata 2018/11/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_CATEGORY_ID_S0100020 = "S01";

        #endregion

        #region TAG入力・変更画面メニューID

        /// --------------------------------------------------
        /// <summary>
        /// TAG入力・変更画面メニューID
        /// </summary>
        /// <create>T.Nakata 2018/11/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_MENU_ID_S0100020 = "S0100020";

        #endregion


        #endregion

        #region 手配明細登録処理
        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細 連携数
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SHUKKA_MEISAI_CNT = "SHUKKA_MEISAI_CNT";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細 出荷数
        /// </summary>
        /// <create>D.Naito 2018/11/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SHUKKA_MEISAI_QTY = "SHUKKA_MEISAI_QTY";

        #endregion

        #region 手配明細照会処理

        #region 納入状態

        /// --------------------------------------------------
        /// <summary>
        /// 納入状態が完了を示すもの
        /// </summary>
        /// <create>D.Okumura 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SURPPLIES_ORDER_FLAG_VALUE3_COMPLETE = "2";

        #endregion

        #region TAG登録可能数

        /// --------------------------------------------------
        /// <summary>
        /// TAG登録可能数
        /// </summary>
        /// <create>D.Okumura 2019/12/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TAG_TOUROKU_MAX = "TAG_TOUROKU_MAX";

        #endregion

        #region ASSY親組み立て残数

        /// --------------------------------------------------
        /// <summary>
        /// ASSY親組み立て残数
        /// </summary>
        /// <create>D.Okumura 2020/01/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ASSY_OYA_ZAN = "ASSY_OYA_ZAN";

        #endregion

        #region 手配明細凡例ファイル名

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細凡例ファイル名
        /// </summary>
        /// <create>D.Okumura 2019/12/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string LEGEND_FILENAME1 = "legend1.xlsx";

        #endregion

        #endregion // 手配明細照会処理

        #region 手配入荷検品処理

        #region 取得テーブルのフィールド名
        /// --------------------------------------------------
        /// <summary>
        /// 手配入荷検品：PC ONLYのフィールド名(手配検品リスト※1:PC Only, 0:Not PC Only)
        /// </summary>
        /// <create>D.Naito 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_PC_ONLY = "PC_ONLY";
        /// --------------------------------------------------
        /// <summary>
        /// 手配入荷検品：残数のフィールド名
        /// </summary>
        /// <create>D.Naito 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ZAN_QTY = "ZAN_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 手配入荷検品：手配実績数(入荷数)のフィールド名
        /// </summary>
        /// <create>D.Naito 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ARRIVAL_ACTUAL_QTY = "ARRIVAL_ACTUAL_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 手配入荷検品：手配明細テーブルのバージョンフィールド名
        /// </summary>
        /// <create>D.Naito 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TM_VERSION = "TM_VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 手配入荷検品：SKS手配明細テーブルのバージョンフィールド名
        /// </summary>
        /// <create>D.Naito 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TS_VERSION = "TS_VERSION";

        #endregion

        #region 手配入荷検品リスト帳票

        /// --------------------------------------------------
        /// <summary>
        /// 手配検品リストレポートクラス名
        /// </summary>
        /// <create>D.Okumura 2018/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string REPORT_R0100120_CLASS_NAME = "SMS.R01.RepTehaiKenpinList";

        /// --------------------------------------------------
        /// <summary>
        /// 手配検品リストドキュメント名
        /// </summary>
        /// <create>D.Okumura 2018/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string REPORT_R0100120_DOCUMENT_NAME { get { return Resources.ComDefine_KenpinListName;  } }

        #endregion

        #region 手配入荷検品リスト名

        /// --------------------------------------------------
        /// <summary>
        /// 手配検品リスト名
        /// </summary>
        /// <create>S.Furugo 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAIKENPIN_LIST = "TAGRENKEI_LIST";

        #endregion

        #region 連携No

        /// --------------------------------------------------
        /// <summary>
        /// 連携No
        /// </summary>
        /// <create>S.Furugo 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAIKENPIN_RENKEI_NO = "TEHAI_RENKEI_NO";

        #endregion

        #region 入荷数

        /// --------------------------------------------------
        /// <summary>
        /// 入荷数
        /// </summary>
        /// <create>S.Furugo 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAIKENPIN_ARRIVAL_QTY = "ARRIVAL_QTY";

        #endregion

        #region バージョン

        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>S.Furugo 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TEHAIKENPIN_VERSION = "VERSION";

        #endregion

        #region SKS発注状態完納

        /// --------------------------------------------------
        /// <summary>
        /// SKS発注状態完納
        /// </summary>
        /// <create>D.Okumura 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HACCHU_FLAG_VALUE3_DELIVERED = "2";

        #endregion

        #region SKS発注状態分納
        /// --------------------------------------------------
        /// <summary>
        /// SKS発注状態分納
        /// </summary>
        /// <create>D.Okumura 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HACCHU_FLAG_VALUE3_SEPARATE_DELIVERED = "3";

        #endregion

        #endregion // 手配入荷検品処理

        #region 手配見積明細

        #region 集計テーブル名(手配見積)

        /// --------------------------------------------------
        /// <summary>
        /// 集計テーブル名
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_TEHAI_ESTIMATE_SUM = "TEHAI_ESTIMATE_SUM";

        #endregion

        #region ER(JPY)

        /// --------------------------------------------------
        /// <summary>
        /// ER(JPY)
        /// </summary>
        /// <create>S.Furugo 2018/11/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly decimal RATE_JPY = 1m;

        #endregion

        #region 通貨単位名

        /// --------------------------------------------------
        /// <summary>
        /// 通貨単位フィールド名
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_CURRENCY_FLAG_NAME = Def_T_TEHAI_ESTIMATE.CURRENCY_FLAG + "_NAME";

        #endregion

        #region 数量単位名

        /// --------------------------------------------------
        /// <summary>
        /// 数量単位フィールド名
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_QUANTITY_UNIT_NAME = Def_T_TEHAI_MEISAI.QUANTITY_UNIT + "_NAME";

        #endregion

        #region 単価*販管費(手配見積)

        /// --------------------------------------------------
        /// <summary>
        /// 単価*販管費のフィールド名
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_UNIT_PRICE_SALSE = "UNIT_PRICE_SALSE";

        #endregion

        #region 単価RMB(手配見積)

        /// --------------------------------------------------
        /// <summary>
        /// 単価RMBのフィールド名
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_UNIT_PRICE_RMB = "UNIT_PRICE_RMB";

        #endregion

        #region パーツ費 Total RMB(手配見積)

        /// --------------------------------------------------
        /// <summary>
        /// パーツ費 Total RMBのフィールド名
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SUM_RMB = "SUM_RMB";

        #endregion

        #region 運賃込 単価 RMB(手配見積)

        /// --------------------------------------------------
        /// <summary>
        /// 運賃込 単価 RMBのフィールド名
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ROB_UNIT_PRICE_RMB = "ROB_UNIT_PRICE_RMB";

        #endregion

        #region 運賃込 Total RMB(手配見積)

        /// --------------------------------------------------
        /// <summary>
        /// 運賃込 Total RMBのフィールド名
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ROB_SUM_RMB = "ROB_SUM_RMB";

        #endregion

        #region 運賃 Total(手配見積)

        /// --------------------------------------------------
        /// <summary>
        /// 運賃 Totalのフィールド名
        /// </summary>
        /// <create>S.Furugo 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SUM_ROB = "SUM_ROB";

        #endregion

        #region 仕切り金額(手配見積)

        /// --------------------------------------------------
        /// <summary>
        /// 仕切り金額のフィールド名
        /// </summary>
        /// <create>J.Chen 2023/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_PAMOUNT_SUM_RMB = "PAMOUNT_SUM_RMB";

        #endregion

        #region 見積状態の色

        /// --------------------------------------------------
        /// <summary>
        /// 見積状態の色
        /// </summary>
        /// <create>D.Okumura 2018/12/2!</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ESTIMATE_COLOR = "ESTIMATE_COLOR";

        #endregion

        #region グレーアウトの色

        /// --------------------------------------------------
        /// <summary>
        /// グレーアウトの色
        /// </summary>
        /// <create>R.Sumi 2022/03/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GRY_COLOR = "000000,888888";

        #endregion

        #region 紫アウトの色

        /// --------------------------------------------------
        /// <summary>
        /// 紫アウトの色
        /// </summary>
        /// <create>Y.Gwon 2023/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PURPLE_COLOR = "000000,D1C4E9";

        #endregion

        #region 出荷済の色

        /// --------------------------------------------------
        /// <summary>
        /// 出荷済の色
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string BLUE_COLOR = "000000,99CCFF";

        #endregion

        #region 無償の色

        /// --------------------------------------------------
        /// <summary>
        /// 無償の色
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GRATIS_COLOR = "000000,99FF99";

        #endregion

        #region 有償の色

        /// --------------------------------------------------
        /// <summary>
        /// 有償の色
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ONEROUS_COLOR = "FF0000,FFCCFF";

        #endregion

        #region 未指定の色

        /// --------------------------------------------------
        /// <summary>
        /// 未指定の色
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NEUTRAL_COLOR = "000000,FFFFFF";

        #endregion

        #region 無償フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 無償フラグ
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GRATIS_FLAG = "0";

        #endregion

        #region 有償フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 有償フラグ
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string ONEROUS_FLAG = "1";

        #endregion

        #region 未指定フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 未指定フラグ
        /// </summary>
        /// <create>H.Iimuro 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string NEUTRAL_FLAG = "9";

        #endregion

        #region 見積書用テーブル名(手配見積)

        /// --------------------------------------------------
        /// <summary>
        /// 見積書用テーブル名
        /// </summary>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_QUOTATION_TABLE = "QUOTATION_TABLE";

        #endregion

        #region 荷受先名

        /// --------------------------------------------------
        /// <summary>
        /// 荷受先名
        /// </summary>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_CONSIGN_NAME = "CONSIGN_NAME";

        #endregion

        #region 出荷国

        /// --------------------------------------------------
        /// <summary>
        /// 出荷国
        /// </summary>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_SHIPPING_LOCATION = "SHIPPING_LOCATION";

        #endregion

        #region 見積発行日

        /// --------------------------------------------------
        /// <summary>
        /// 見積発行日
        /// </summary>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ISSUE_DATE = "ISSUE_DATE";

        #endregion

        #region 見積発行年月

        /// --------------------------------------------------
        /// <summary>
        /// 見積発行年月
        /// </summary>
        /// <create>J.Chen 2024/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ISSUE_YEARMONTH = "FLD_ISSUE_YEARMONTH";

        #endregion

        #region 見積書出力先フォルダ

        /// --------------------------------------------------
        /// <summary>
        /// 見積書出力先フォルダ
        /// </summary>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string QUOTATION_OUTPUT_DIR = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"sms\Quotation");

        #endregion

        #region 見積名称

        /// --------------------------------------------------
        /// <summary>
        /// 見積名称
        /// </summary>
        /// <create>J.Chen 2024/02/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ESTIMATE_NAME = "ESTIMATE_NAME";

        #endregion

        #endregion //手配見積明細

        #region 荷姿関連

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿Excel出力先フォルダ
        /// </summary>
        /// <create>H.Tajimi 2018/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PACKING_EXCEL_OUTPUT_DIR = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"sms\Packing");
        /// --------------------------------------------------
        /// <summary>
        /// サイズ分離時の記号
        /// </summary>
        /// <create>H.Tajimi 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SPLIT_SIZE_CHAR = "x";
        /// --------------------------------------------------
        /// <summary>
        /// GRWTの単位
        /// </summary>
        /// <create>H.Tajimi 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string GRWT_UNIT = "kg";
        /// --------------------------------------------------
        /// <summary>
        /// カートン数
        /// </summary>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_CARTON_QTY = "CARTON_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// カートンNo
        /// </summary>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_CARTON_NO = "CARTON_NO";
        /// --------------------------------------------------
        /// <summary>
        /// パレット数
        /// </summary>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_PALLET_QTY = "PALLET_QTY";
        /// --------------------------------------------------
        /// <summary>
        /// 梱包No.
        /// </summary>
        /// <create>K.Harada 2022/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_KONPO_NO = "KONPO_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 荷姿用のユーザマスタ
        /// </summary>
        /// <create>H.Tajimi 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DT_PACKING_M_USER = "PACKING_M_USER";
        /// --------------------------------------------------
        /// <summary>
        /// TAG連携用のユーザマスタ
        /// </summary>
        /// <create>H.Tajimi 2019/08/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DT_TAG_RENKEI_M_USER = "TAG_RENKEI_M_USER";

        #endregion

        #region INVOICE

        /// --------------------------------------------------
        /// <summary>
        /// 宛先名称(CONSIGNEE)の最大行数
        /// </summary>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int MAX_LINE_LENGTH_CONSIGNEE = 7;
        /// --------------------------------------------------
        /// <summary>
        /// 宛先名称(DELIVERY)の最大行数
        /// </summary>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly int MAX_LINE_LENGTH_DELIVERY = 9;

        #endregion

        #region 物件名保守一括

        /// --------------------------------------------------
        /// <summary>
        /// 物件用データテーブル名
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_DISP_BUKKEN = "DISP_BUKKEN_TBL";
        /// --------------------------------------------------
        /// <summary>
        /// 本体 物件管理NO
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_NORMAL_BUKKEN_NO = "NORMAL_BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 本体 発行済TagNO
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_NORMAL_ISSUED_TAG_NO = "NORMAL_ISSUED_TAG_NO";
        /// --------------------------------------------------
        /// <summary>
        /// 本体 保守バージョン
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_NORMAL_MAINTE_VERSION = "NORMAL_MAINTE_VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// 本体 メール通知運用
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_NORMAL_MAIL_NOTIFY = "NORMAL_MAIL_NOTIFY";
        /// --------------------------------------------------
        /// <summary>
        /// AR 物件管理NO
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_AR_BUKKEN_NO = "AR_BUKKEN_NO";
        /// --------------------------------------------------
        /// <summary>
        /// AR 発行済TagNO
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_AR_ISSUED_TAG_NO = "AR_ISSUED_TAG_NO";
        /// --------------------------------------------------
        /// <summary>
        /// AR 保守バージョン
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_AR_MAINTE_VERSION = "AR_MAINTE_VERSION";
        /// --------------------------------------------------
        /// <summary>
        /// AR メール通知運用
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_AR_MAIL_NOTIFY = "AR_MAIL_NOTIFY";
        /// --------------------------------------------------
        /// <summary>
        /// AR メール通知運用名称
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_AR_MAIL_NOTIFY_NAME = "AR_MAIL_NOTIFY_NAME";

        #endregion

        #region 木枠梱包用

        /// --------------------------------------------------
        /// <summary>
        /// 便間移動パレットデータ
        /// </summary>
        /// <create>H.Tajimi 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_MOVE_SHIP_PALLET = "MOVE_SHIP_PALLET";
        /// --------------------------------------------------
        /// <summary>
        /// 便間移動タグデータ
        /// </summary>
        /// <create>H.Tajimi 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_MOVE_SHIP_TAG = "MOVE_SHIP_TAG";

        #endregion

        #region SKS手配連携

        /// --------------------------------------------------
        /// <summary>
        /// 発注フラグ名
        /// </summary>
        /// <create>H.Tajimi 2019/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_HACCHU_FLAG_NAME = "HACCHU_FLAG_NAME";

        #endregion

        #region 担当者マスタ

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称
        /// </summary>
        /// <create>H.Tsuji 2019/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_LIST_FLAG_NAME = "LIST_FLAG_NAME";
        /// --------------------------------------------------
        /// <summary>
        /// メールフラグ名称
        /// </summary>
        /// <create>H.Tsuji 2019/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_MAIL_ADDRESS_FLAG_NAME = "MAIL_ADDRESS_FLAG_NAME";

        #endregion
        
        #region AR情報で使用するテーブル・フィールド
        /// --------------------------------------------------
        /// <summary>
        /// 結果ARNo.のフィールド名
        /// </summary>
        /// <create>T.Nukaga 2019/11/21 AR7000番運用対応</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_KEKKA_AR_NO = "KEKKA_AR_NO";
        #endregion

        #region AR進捗管理で使用するテーブル・フィールド

        /// --------------------------------------------------
        /// <summary>
        /// AR進捗情報受け渡し用テーブル名(追加用)
        /// </summary>
        /// <create>D.Okumura 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_ARSHINCHOKU_ADD = Def_T_AR_SHINCHOKU.Name + "_ADD";
        /// --------------------------------------------------
        /// <summary>
        /// AR進捗情報受け渡し用テーブル名(削除用)
        /// </summary>
        /// <create>D.Okumura 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_ARSHINCHOKU_DEL = Def_T_AR_SHINCHOKU.Name + "_DELETE";

        /// --------------------------------------------------
        /// <summary>
        /// ARメール送信情報受け渡し用テーブル名
        /// </summary>
        /// <create>D.Okumura 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_AR_MAILINFO = "AR_MAILINFO";
        /// --------------------------------------------------
        /// <summary>
        /// AR進捗メール送信情報受け渡し用テーブル名
        /// </summary>
        /// <create>T.Nakata 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_ARSHINCHOKU_MAILINFO = "ARSHINCHOKU_MAILINFO";

        /// --------------------------------------------------
        /// <summary>
        /// ARNoのフィールド名
        /// </summary>
        /// <create>T.Nakata 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ARSHINCHOKU_ARNO = "ARNO";

        #endregion

        #region ＡＲ進捗管理日付登録で使用するテーブル・フィールド

        /// --------------------------------------------------
        /// <summary>
        /// フォーム表示情報用テーブル名
        /// </summary>
        /// <create>T.Nakata 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string DTTBL_ARSHINCHOKU_DT = "ARSHINCHOKU_DT";

        /// --------------------------------------------------
        /// <summary>
        /// 表示用日付のフィールド名
        /// </summary>
        /// <create>T.Nakata 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ARSHINCHOKU_DT_DATE = "ARSHINCHOKU_DT_DATE";

        #endregion

        #region 出荷計画照会で使用するテーブル・フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 表示用TAG発行状況のフィールド名
        /// </summary>
        /// <create>J.Chen 2023/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TAG_NUM = "TAG_NUM";

        /// --------------------------------------------------
        /// <summary>
        /// 表示用TAGステータスのフィールド名
        /// </summary>
        /// <create>J.Chen 2023/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_TAG_STATUS = "TAG_STATUS";

        /// --------------------------------------------------
        /// <summary>
        /// 表示用現場用ステータスのフィールド名
        /// </summary>
        /// <create>J.Chen 2024/10/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_GENBA_YO_STATUS_NAME = "GENBA_YO_STATUS_NAME";
        
        #endregion

        #region 出荷計画表パス

        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画取組Excel出力先フォルダ
        /// </summary>
        /// <create>Y.Gwon 2023/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string PLANNING_EXCEL_OUTPUT_DIR = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"sms\Planning");

        # endregion

        #region HT引渡作業

        /// --------------------------------------------------
        /// <summary>
        /// 新規のITEM_NAME
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHINKI_JP_NAME = "";
        /// --------------------------------------------------
        /// <summary>
        /// 発行済のITEM_NAME
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAGHAKKOZUMI_JP_NAME = "発行済";
        /// --------------------------------------------------
        /// <summary>
        /// 発行済のITEM_NAME
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string TAGHAKKOZUMI_US_NAME = "Issued tag";
        /// --------------------------------------------------
        /// <summary>
        /// 引渡済のITEM_NAME
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HIKIWATASHIZUMI_JP_NAME = "引渡済";
        /// --------------------------------------------------
        /// <summary>
        /// 引渡済のITEM_NAME
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HIKIWATASHIZUMI_US_NAME = "Handover";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷済のITEM_NAME
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKAZUMI_JP_NAME = "出荷済";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷済のITEM_NAME
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string SHUKKAZUMI_US_NAME = "Shipped";
        /// --------------------------------------------------
        /// <summary>
        /// 受入済のITEM_NAME
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIREZUMI_JP_NAME = "受入済";
        /// --------------------------------------------------
        /// <summary>
        /// 受入済のITEM_NAME
        /// </summary>
        /// <remarks></remarks>
        /// <create>T.SASAYAMA 2023/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string UKEIREZUMI_US_NAME = "Received";
        /// --------------------------------------------------
        /// <summary>
        /// 保留の値2
        /// </summary>
        /// <remarks></remarks>
        /// <create>AutoCodeGenerator 2022/11/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string HORYU_VALUE2 = "2  ";

        #endregion

        #region VALUE照会

        /// --------------------------------------------------
        /// <summary>
        /// 見積回数
        /// </summary>
        /// <create>J.Chen 2024/02/22/create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_ESTIMATION_COUNT = "CNT";

        /// --------------------------------------------------
        /// <summary>
        /// 使用済みValue合計
        /// </summary>
        /// <create>J.Chen 2024/02/22/create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_USED_VALUE = "USED_VALUE";

        /// --------------------------------------------------
        /// <summary>
        /// 残Value
        /// </summary>
        /// <create>J.Chen 2024/02/22/create>
        /// <update></update>
        /// --------------------------------------------------
        public static readonly string FLD_REMAINING_VALUE = "REMAINING_VALUE";

        #endregion
    }
}
