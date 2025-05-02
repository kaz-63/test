using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SystemBase.Excel
{
    /// --------------------------------------------------
    /// <summary>
    /// エクスポート情報コレクション
    /// </summary>
    /// <create>Y.Higuchi 2010/07/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportInfoCollection : Collection<ExportInfo>
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダーを出力するかどうか
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isHeader = true;
        /// --------------------------------------------------
        /// <summary>
        /// ラインを出力するかどうか
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isLine = true;
        /// --------------------------------------------------
        /// <summary>
        /// 出力開始列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _startCol = 0;
        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ表示開始行インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _headerStartRow = 0;
        /// --------------------------------------------------
        /// <summary>
        /// データ表示開始行インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _dataStartRow = 1;

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportInfoCollection()
            : base()
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="list">新しいコレクションにラップされているリスト</param>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportInfoCollection(IList<ExportInfo> list)
            : base(list)
        {
        }

        #endregion

        #region Properties

        #region ヘッダーを出力するかどうか

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダーを出力するかどうか
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsHeader
        {
            get { return this._isHeader; }
            set { this._isHeader = value; }
        }

        #endregion

        #region ラインを出力するかどうか

        /// --------------------------------------------------
        /// <summary>
        /// ラインを出力するかどうか
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsLine
        {
            get { return this._isLine; }
            set { this._isLine = value; }
        }

        #endregion

        #region 出力開始列インデックス

        /// --------------------------------------------------
        /// <summary>
        /// 出力開始列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public int StartCol
        {
            get { return this._startCol; }
            set { this._startCol = value; }
        }

        #endregion

        #region ヘッダ表示開始行インデックス

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ表示開始行インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public int HeaderStartRow
        {
            get { return this._headerStartRow; }
            set { this._headerStartRow = value; }
        }

        #endregion

        #region データ表示開始行インデックス

        /// --------------------------------------------------
        /// <summary>
        /// データ表示開始行インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public int DataStartRow
        {
            get { return this._dataStartRow; }
            set { this._dataStartRow = value; }
        }

        #endregion

        #endregion

        #region Add

        /// --------------------------------------------------
        /// <summary>
        /// 末尾にコレクションを追加
        /// </summary>
        /// <param name="dataColName">データの列名</param>
        /// <param name="caption">ヘッダ文字列</param>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public void Add(String dataColName, string caption)
        {
            this.Add(dataColName, caption, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 末尾にコレクションを追加
        /// </summary>
        /// <param name="dataColName">データの列名</param>
        /// <param name="caption">ヘッダ文字列</param>
        /// <param name="dateTimeFormat">日付型の場合のフォーマット</param>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public void Add(string dataColName, string caption, string dateTimeFormat)
        {
            ExportInfo expInfo = new ExportInfo();
            // 出力データタイプ
            expInfo.DataType = ExportDataType.Other;
            // データの列名
            expInfo.DataColName = dataColName;
            // ヘッダ文字列
            expInfo.Caption = caption;
            // 日付型の場合のフォーマット
            expInfo.DateTimeFormat = dateTimeFormat;
            this.Add(expInfo);
        }


        /// --------------------------------------------------
        /// <summary>
        /// 末尾にコレクションを追加
        /// </summary>
        /// <param name="dataColName">データの列名</param>
        /// <param name="caption">ヘッダ文字列</param>
        /// <param name="dateTimeFormat">日付型の場合のフォーマット</param>
        /// <param name="dataType">データタイプ</param>
        /// <create>J.Chen 2023/12/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public void Add(string dataColName, string caption, string dateTimeFormat, ExportDataType dataType)
        {
            ExportInfo expInfo = new ExportInfo();
            // 出力データタイプ
            expInfo.DataType = dataType;
            // データの列名
            expInfo.DataColName = dataColName;
            // ヘッダ文字列
            expInfo.Caption = caption;
            // 日付型の場合のフォーマット
            expInfo.DateTimeFormat = dateTimeFormat;
            this.Add(expInfo);
        }

        #endregion
    }
}
