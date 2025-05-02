using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Commons;
using SystemBase.Excel;
using SMS.E01.Properties;

namespace SMS.E01
{
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗管理Excel出力クラス
    /// </summary>
    /// <create>T.Nakata 2019/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportShinchokuKanri : BaseExportXlsx
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Nakata 2019/07267</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportShinchokuKanri()
            : base()
        {
        }

        #endregion

        #region AR進捗管理出力

        /// --------------------------------------------------
        /// <summary>
        /// AR進捗管理出力
        /// </summary>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, DataTable dt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();
                for (int i = 0; i < dt.Columns.Count; i++ )
                    expInfoCollection.Add(dt.Columns[i].ToString(), dt.Columns[i].ToString());
                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {   // 進捗DataのExcelFileを出力しました。
                    msgID = "A0100050007";
                }
                return ret;
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(msgID))
                {   // Excel出力に失敗しました。
                    msgID = "A7777777001";
                }
                return false;
            }
        }

        #endregion

    }
}
