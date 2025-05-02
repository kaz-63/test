using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Commons;
using SystemBase.Excel;
using SMS.E01.Properties;

namespace SMS.E01
{
    /// --------------------------------------------------
    /// <summary>
    /// パーツ翻訳名Excel出力クラス
    /// </summary>
    /// <create>H.Tajimi 2019/07/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportPartsName : BaseExportXlsx
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tajimi 2019/07/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportPartsName()
            : base()
        {
        }

        #endregion

        #region パーツ翻訳名Excelの出力

        /// --------------------------------------------------
        /// <summary>
        /// パーツ翻訳名Excelの出力
        /// </summary>
        /// <param name="filePath">Excelファイルパス</param>
        /// <param name="dt">DataTable</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">メッセージ引数</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/07/03</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, System.Data.DataTable dt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                string colName = string.Empty;
                string colCaption = string.Empty;
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();

                // 型式
                colName = Def_M_PARTS_NAME.PARTS_CD;
                colCaption = Resources.ExportPartsNameHonyaku_Type;
                expInfoCollection.Add(colName, colCaption);
                // 品名(和文)
                colName = Def_M_PARTS_NAME.HINMEI_JP;
                colCaption = Resources.ExportPartsNameHonyaku_JPName;
                expInfoCollection.Add(colName, colCaption);
                // 品名
                colName = Def_M_PARTS_NAME.HINMEI;
                colCaption = Resources.ExportPartsNameHonyaku_Name;
                expInfoCollection.Add(colName, colCaption);
                // INV付加名
                colName = Def_M_PARTS_NAME.HINMEI_INV;
                colCaption = Resources.ExportPartsNameHonyaku_InvName;
                expInfoCollection.Add(colName, colCaption);
                // Maker
                colName = Def_M_PARTS_NAME.MAKER;
                colCaption = Resources.ExportPartsNameHonyaku_Maker;
                expInfoCollection.Add(colName, colCaption);
                // 原産国
                colName = Def_M_PARTS_NAME.FREE2;
                colCaption = Resources.ExportPartsNameHonyaku_OriginCountry;
                expInfoCollection.Add(colName, colCaption);
                // 取引先
                colName = Def_M_PARTS_NAME.SUPPLIER;
                colCaption = Resources.ExportPartsNameHonyaku_Customer;
                expInfoCollection.Add(colName, colCaption);
                // 備考
                colName = Def_M_PARTS_NAME.NOTE;
                colCaption = Resources.ExportPartsNameHonyaku_Notes;
                expInfoCollection.Add(colName, colCaption);
                // 通関確認状態
                colName = Def_M_PARTS_NAME.CUSTOMS_STATUS;
                colCaption = Resources.ExportPartsNameHonyaku_CustomsStatus;
                expInfoCollection.Add(colName, colCaption);

                var ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // パーツ名翻訳Excelファイルを出力しました。
                    msgID = "M0100130007";
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

        #endregion
    }
}
