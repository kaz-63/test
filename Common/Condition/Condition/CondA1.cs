using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// A01用コンディション
    /// </summary>
    /// <create>M.Tsutsumi 2010/08/11</create>
    /// <update>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</update>
    /// <update>J.Chen 2024/08/06 ARメール添付ファイル対応</update>
    /// --------------------------------------------------
    public class CondA1 : CondBase
    {

        #region Fields

        //出荷区分
        private string _shukkaFlag = null;
        //納入先コード
        private string _nonyusakiCD = null;
        //納入先
        private string _nonyusakiName = null;
        // リスト区分
        private string _listFlag = null;
        // ARNO
        private string _arNo = null;
        // 状況区分
        private string _jyokyoFlag = null;
        // 状況区分(AR画面用)
        private string _jyokyoFlagAr = null;
        // 対策内容
        private string _taisaku = null;
        // 技連NO
        private string _girenNo = null;
        // 現地・手配先
        private string _genchiTehaisaki = null;

        //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
        // リスト区分名称
        private string _listFlagName0 = null;
        private string _listFlagName1 = null;
        private string _listFlagName2 = null;
        private string _listFlagName3 = null;
        private string _listFlagName4 = null;
        private string _listFlagName5 = null;
        private string _listFlagName6 = null;
        private string _listFlagName7 = null;
        // @@@ ↑

        // 更新日時(開始)
        private DateTime? _updateDateFrom = null;
        // 更新日時(終了)
        private DateTime? _updateDateTo = null;
        // 機種
        private string _kishu = null;
        // 号機
        private string _goki = null;
        // 範囲の区切り文字
        private char _separatorRange = ' ';
        // 項目の区切り文字
        private char _separatorItem = ' ';
        // 日付区分
        private string _dateKubun = null;

        // 進捗区分
        private bool _isShinchoku = false;

        // 現地出荷状況
        private string _isgenchiShukkaJyokyo = null;

        // 予定メールID
        private string _mailIDTemp = null;
        // 予定ARNo
        private string _arNoTemp = null;
        // 予定添付ファイルパス
        private string _filePathTemp = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondA1()
            : this(new LoginInfo())
        {
            
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">ログインユーザー情報</param>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public CondA1(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaFlag
        {
            get
            {
                return _shukkaFlag;
            }
            set
            {
                _shukkaFlag = value;
            }
        }

        #endregion

        #region 納入先コード

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCD
        {
            get
            {
                return _nonyusakiCD;
            }
            set
            {
                _nonyusakiCD = value;
            }
        }

        #endregion

        #region 納入先

        /// --------------------------------------------------
        /// <summary>
        /// 納入先
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiName
        {
            get
            {
                return _nonyusakiName;
            }
            set
            {
                _nonyusakiName = value;
            }
        }

        #endregion

        #region リスト区分

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlag
        {
            get
            {
                return _listFlag;
            }
            set
            {
                _listFlag = value;
            }
        }

        #endregion

        #region ARNO

        /// --------------------------------------------------
        /// <summary>
        /// ARNO
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ArNo
        {
            get
            {
                return _arNo;
            }
            set
            {
                _arNo = value;
            }
        }

        #endregion

        #region 状況区分

        /// --------------------------------------------------
        /// <summary>
        /// 状況区分
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string JyokyoFlag
        {
            get
            {
                return _jyokyoFlag;
            }
            set
            {
                _jyokyoFlag = value;
            }
        }

        #endregion

        #region 状況区分(AR画面用)

        /// --------------------------------------------------
        /// <summary>
        /// 状況区分(AR画面用)
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string JyokyoFlagAR
        {
            get
            {
                return _jyokyoFlagAr;
            }
            set
            {
                _jyokyoFlagAr = value;
            }
        }

        #endregion

        #region 対策内容

        /// --------------------------------------------------
        /// <summary>
        /// 対策内容
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Taisaku
        {
            get
            {
                return _taisaku;
            }
            set
            {
                _taisaku = value;
            }
        }

        #endregion

        #region 技連NO

        /// --------------------------------------------------
        /// <summary>
        /// 技連NO
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GirenNo
        {
            get
            {
                return _girenNo;
            }
            set
            {
                _girenNo = value;
            }
        }

        #endregion

        #region 現地・手配先

        /// --------------------------------------------------
        /// <summary>
        /// 現地・手配先
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GenchiTehaisaki
        {
            get
            {
                return _genchiTehaisaki;
            }
            set
            {
                _genchiTehaisaki = value;
            }
        }

        #endregion

        #region リスト区分名称０

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称０
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName0
        {
            get
            {
                return _listFlagName0;
            }
            set
            {
                _listFlagName0 = value;
            }
        }

        #endregion

        #region リスト区分名称１

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称１
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName1
        {
            get
            {
                return _listFlagName1;
            }
            set
            {
                _listFlagName1 = value;
            }
        }

        #endregion

        #region リスト区分名称２

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称２
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName2
        {
            get
            {
                return _listFlagName2;
            }
            set
            {
                _listFlagName2 = value;
            }
        }

        #endregion

        #region リスト区分名称３

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称３
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName3
        {
            get
            {
                return _listFlagName3;
            }
            set
            {
                _listFlagName3 = value;
            }
        }

        #endregion

        #region リスト区分名称４

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称４
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName4
        {
            get
            {
                return _listFlagName4;
            }
            set
            {
                _listFlagName4 = value;
            }
        }

        #endregion

        #region リスト区分名称５

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称５
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName5
        {
            get
            {
                return _listFlagName5;
            }
            set
            {
                _listFlagName5 = value;
            }
        }

        #endregion

        #region リスト区分名称６

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称６
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName6
        {
            get
            {
                return _listFlagName6;
            }
            set
            {
                _listFlagName6 = value;
            }
        }

        #endregion

        #region リスト区分名称７

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分名称７
        /// </summary>
        /// <create>M.Tsutsumi 2011/02/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlagName7
        {
            get
            {
                return _listFlagName7;
            }
            set
            {
                _listFlagName7 = value;
            }
        }

        #endregion

        #region 更新日時(開始)

        /// --------------------------------------------------
        /// <summary>
        /// 更新日時(開始)
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public DateTime? UpdateDateFrom
        {
            get
            {
                return _updateDateFrom;
            }
            set
            {
                _updateDateFrom = value;
            }
        }

        #endregion

        #region 更新日時(終了)

        /// --------------------------------------------------
        /// <summary>
        /// 更新日時(終了)
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public DateTime? UpdateDateTo
        {
            get
            {
                return _updateDateTo;
            }
            set
            {
                _updateDateTo = value;
            }
        }

        #endregion

        #region 機種

        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Kishu
        {
            get
            {
                return _kishu;
            }
            set
            {
                _kishu = value;
            }
        }

        #endregion

        #region 号機

        /// --------------------------------------------------
        /// <summary>
        /// 号機
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Goki
        {
            get
            {
                return _goki;
            }
            set
            {
                _goki = value;
            }
        }

        #endregion

        #region 範囲の区切り文字

        /// --------------------------------------------------
        /// <summary>
        /// 範囲の区切り文字
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public char SeparatorRange
        {
            get
            {
                return _separatorRange;
            }
            set
            {
                _separatorRange = value;
            }
        }

        #endregion

        #region 項目の区切り文字

        /// --------------------------------------------------
        /// <summary>
        /// 項目の区切り文字
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public char SeparatorItem
        {
            get
            {
                return _separatorItem;
            }
            set
            {
                _separatorItem = value;
            }
        }

        #endregion

        #region 日付区分

        /// --------------------------------------------------
        /// <summary>
        /// 日付区分
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DateKubun
        {
            get
            {
                return _dateKubun;
            }
            set
            {
                _dateKubun = value;
            }
        }

        #endregion

        #region 選択グループコード

        /// --------------------------------------------------
        /// <summary>
        /// 選択グループコード
        /// </summary>
        /// <create>T.Sakiori 2012/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SelectGroupCode { get; set; }

        #endregion

        #region 物件管理No

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>T.Sakiori 2012/04/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenNo { get; set; }

        #endregion

        #region 明細登録ならtrue、編集ならfalse

        /// --------------------------------------------------
        /// <summary>
        /// 明細登録ならtrue、編集ならfalse
        /// </summary>
        /// <create>T.Sakiori 2017/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsToroku { get; set; }

        #endregion

        #region 進捗管理区分

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理区分
        /// </summary>
        /// <create>D.Okumura 2019/08/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsShinchoku
        {
            get
            {
                return _isShinchoku;
            }
            set
            {
                _isShinchoku = value;
            }
        }

        #endregion

        #region 出荷状況

        /// --------------------------------------------------
        /// <summary>
        /// 出荷状況
        /// </summary>
        /// <create>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</create>
        /// <update></update>
        /// --------------------------------------------------
        public string IsGenchiShukkaJyokyo
        {
            get
            {
                return _isgenchiShukkaJyokyo;
            }
            set
            {
                _isgenchiShukkaJyokyo = value;
            }
        }

        #endregion

        #region 予定メールID

        /// --------------------------------------------------
        /// <summary>
        /// 予定メールID
        /// </summary>
        /// <create>J.Chen 2024/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public string MailIDTemp
        {
            get
            {
                return _mailIDTemp;
            }
            set
            {
                _mailIDTemp = value;
            }
        }

        #endregion

        #region 予定ARNo

        /// --------------------------------------------------
        /// <summary>
        /// 予定ARNo
        /// </summary>
        /// <create>J.Chen 2024/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ArNoTemp
        {
            get
            {
                return _arNoTemp;
            }
            set
            {
                _arNoTemp = value;
            }
        }

        #endregion

        #region 予定添付ファイルパス

        /// --------------------------------------------------
        /// <summary>
        /// 予定添付ファイルパス
        /// </summary>
        /// <create>J.Chen 2024/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public string FilePathTemp
        {
            get
            {
                return _filePathTemp;
            }
            set
            {
                _filePathTemp = value;
            }
        }

        #endregion

    }
}
