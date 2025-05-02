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
    /// 木枠明細Excel出力クラス
    /// </summary>
    /// <create>H.Tsunamura 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportKiwakuMeisai : BaseExport
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tsunamura 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportKiwakuMeisai()
            : base()
        {
        }

        #endregion

        #region 木枠明細

        /// --------------------------------------------------
        /// <summary>
        /// 木枠明細Excelの出力
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsunamura 2010/08/03</create>
        /// <update>K.Tsutsumi 2013/06/20 MMNET → MMENT</update>
        /// <update>J.Chen 2023/12/15 Decimal指定追加</update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, System.Data.DataTable dt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                string colName = string.Empty;
                string colCaption = string.Empty;
                ExportDataType colDecimalDataType = ExportDataType.Decimal;
                
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();
                // 工事識別No
                colName = Def_T_KIWAKU.KOJI_NO;
                colCaption = Resources.ExportKiwakuMeisai_ConstructionIdentificationNo;
                expInfoCollection.Add(colName, colCaption);
                // 工事識別名
                colName = Def_T_KIWAKU.KOJI_NAME;
                colCaption = Resources.ExportKiwakuMeisai_ConstructionIdentificationName;
                expInfoCollection.Add(colName, colCaption);
                // 便
                colName = Def_T_KIWAKU.SHIP;
                colCaption = Resources.ExportKiwakuMeisai_Ship;
                expInfoCollection.Add(colName, colCaption);
                
                // C/NO
                colName = Def_T_KIWAKU_MEISAI.CASE_NO;
                colCaption = Resources.ExportKiwakuMeisai_CNo;
                expInfoCollection.Add(colName, colCaption);
                // STYLE
                colName = Def_T_KIWAKU_MEISAI.STYLE;
                colCaption = Resources.ExportKiwakuMeisai_Style;
                expInfoCollection.Add(colName, colCaption);
                // ITEM
                colName = Def_T_KIWAKU_MEISAI.ITEM;
                colCaption = Resources.ExportKiwakuMeisai_Item;
                expInfoCollection.Add(colName, colCaption);
                // DESCRIPTION1
                colName = Def_T_KIWAKU_MEISAI.DESCRIPTION_1;
                colCaption = Resources.ExportKiwakuMeisai_Description1;
                expInfoCollection.Add(colName, colCaption);
                // DESCRIPTION2
                colName = Def_T_KIWAKU_MEISAI.DESCRIPTION_2;
                colCaption = Resources.ExportKiwakuMeisai_Description2;
                expInfoCollection.Add(colName, colCaption);
                // DIMENSION(L)
                colName = Def_T_KIWAKU_MEISAI.DIMENSION_L;
                colCaption = Resources.ExportKiwakuMeisai_DimensionL;
                expInfoCollection.Add(colName, colCaption);
                // DIMENSION(W)
                colName = Def_T_KIWAKU_MEISAI.DIMENSION_W;
                colCaption = Resources.ExportKiwakuMeisai_DimensionW;
                expInfoCollection.Add(colName, colCaption);
                // DIMENSION(H)
                colName = Def_T_KIWAKU_MEISAI.DIMENSION_H;
                colCaption = Resources.ExportKiwakuMeisai_DimensionH;
                expInfoCollection.Add(colName, colCaption);
                // MMNET
                colName = Def_T_KIWAKU_MEISAI.MMNET;
                colCaption = Resources.ExportKiwakuMeisai_Mmnet;
                expInfoCollection.Add(colName, colCaption);
                // NET/W
                colName = Def_T_KIWAKU_MEISAI.NET_W;
                colCaption = Resources.ExportKiwakuMeisai_NetW;
                expInfoCollection.Add(colName, colCaption, null, colDecimalDataType);
                // GROSS･W
                colName = Def_T_KIWAKU_MEISAI.GROSS_W;
                colCaption = Resources.ExportKiwakuMeisai_GrossW;
                expInfoCollection.Add(colName, colCaption, null, colDecimalDataType);
                // 木材重量
                colName = Def_T_KIWAKU_MEISAI.MOKUZAI_JYURYO;
                colCaption = Resources.ExportKiwakuMeisai_WoodWeight;
                expInfoCollection.Add(colName, colCaption, null, colDecimalDataType);
                
                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // 木枠明細Excelファイルを出力しました。
                    msgID = "E0100030001";
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
