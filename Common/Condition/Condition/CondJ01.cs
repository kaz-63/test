using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// J01用コンディション
    /// (ログインは関係ないのでとりあえずCondBaseは継承しない)
    /// </summary>
    /// <create>Y.Higuchi 2010/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class CondJ01
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shoriFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 前回日次処理日時
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _nichijiDate = null;
        /// --------------------------------------------------
        /// <summary>
        /// 前回月次処理日時
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _getsujiDate = null;
        /// --------------------------------------------------
        /// <summary>
        /// 削除対象日時
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _targetDate = null;
        /// --------------------------------------------------
        /// <summary>
        /// エクスポートバックアップパス
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _expBackupPath = null;
        /// --------------------------------------------------
        /// <summary>
        /// 前回SKS連携処理日時
        /// </summary>
        /// <create>H.Tajimi 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _lastestDate = null;
        /// --------------------------------------------------
        /// <summary>
        /// 言語コード
        /// </summary>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _lang = null;

        #endregion

        #region 処理フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShoriFlag
        {
            get { return this._shoriFlag; }
            set { this._shoriFlag = value; }
        }

        #endregion

        #region エクスポートバックアップパス

        /// --------------------------------------------------
        /// <summary>
        /// エクスポートバックアップパス
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ExpBackupPath
        {
            get { return this._expBackupPath; }
            set { this._expBackupPath = value; }
        }

        #endregion
        
        #region 前回日次処理日時

        /// --------------------------------------------------
        /// <summary>
        /// 前回日次処理日時
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public object NichijiDate
        {
            get { return this._nichijiDate; }
            set { this._nichijiDate = value; }
        }

        #endregion

        #region 前回月次処理日時

        /// --------------------------------------------------
        /// <summary>
        /// 前回月次処理日時
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public object GetsujiDate
        {
            get { return this._getsujiDate; }
            set { this._getsujiDate = value; }
        }

        #endregion

        #region 削除対象日時

        /// --------------------------------------------------
        /// <summary>
        /// 削除対象日時
        /// </summary>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public object TargetDate
        {
            get { return this._targetDate; }
            set { this._targetDate = value; }
        }

        #endregion

        #region 前回SKS連携処理日時

        /// --------------------------------------------------
        /// <summary>
        /// 前回SKS連携処理日時
        /// </summary>
        /// <create>H.Tajimi 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public object LastestDate
        {
            get { return this._lastestDate; }
            set { this._lastestDate = value; }
        }

        #endregion

        #region 言語コード

        /// --------------------------------------------------
        /// <summary>
        /// 言語コード
        /// </summary>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Lang
        {
            get { return this._lang; }
            set { this._lang = value; }
        }

        #endregion
    }
}
