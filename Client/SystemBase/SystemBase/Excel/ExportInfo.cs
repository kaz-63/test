using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemBase.Excel
{
    #region Enum

    /// --------------------------------------------------
    /// <summary>
    /// 出力データタイプ
    /// </summary>
    /// <create>Y.Higuchi 2010/07/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public enum ExportDataType
    {
        /// --------------------------------------------------
        /// <summary>
        /// 文字列
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        String = 0,
        /// --------------------------------------------------
        /// <summary>
        /// 日付型
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        DateTime = 1,
        /// --------------------------------------------------
        /// <summary>
        /// その他
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        Other = 2,
        /// --------------------------------------------------
        /// <summary>
        /// その他
        /// </summary>
        /// <create>J.Chen 2023/12/15</create>
        /// <update></update>
        /// --------------------------------------------------
        Decimal = 3,
    }

    #endregion

    /// --------------------------------------------------
    /// <summary>
    /// エクスポート情報
    /// </summary>
    /// <create>Y.Higuchi 2010/07/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportInfo : ICloneable
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 出力データタイプ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private ExportDataType _dataType = ExportDataType.Other;
        /// --------------------------------------------------
        /// <summary>
        /// データの列名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _dataColName = null;
        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ文字列
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _caption = null;
        /// --------------------------------------------------
        /// <summary>
        /// 日付型の場合のフォーマット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _dateTimeFormat = null;

        #endregion

        #region 出力データタイプ

        /// --------------------------------------------------
        /// <summary>
        /// 出力データタイプ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportDataType DataType
        {
            get { return this._dataType; }
            set { this._dataType = value; }
        }

        #endregion

        #region データの列名

        /// --------------------------------------------------
        /// <summary>
        /// データの列名
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DataColName
        {
            get { return this._dataColName; }
            set { this._dataColName = value; }
        }

        #endregion

        #region ヘッダ文字列

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ文字列
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Caption
        {
            get { return this._caption; }
            set { this._caption = value; }
        }

        #endregion

        #region 日付型の場合のフォーマット

        /// --------------------------------------------------
        /// <summary>
        /// 日付型の場合のフォーマット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DateTimeFormat
        {
            get { return this._dateTimeFormat; }
            set { this._dateTimeFormat = value; }
        }

        #endregion

        #region Clone

        /// --------------------------------------------------
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportInfo Clone()
        {
            return (ExportInfo)this.MemberwiseClone();
        }

        #region ICloneable メンバ

        /// --------------------------------------------------
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #endregion
    }
}
