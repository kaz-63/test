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
    /// 担当者Excel出力クラス
    /// </summary>
    /// <create>H.Tajimi 2019/07/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportUserMaster : BaseExportXlsx
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tajimi 2019/07/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportUserMaster()
            : base()
        {
        }

        #endregion

        #region 担当者Excelの出力

        /// --------------------------------------------------
        /// <summary>
        /// 担当者Excelの出力
        /// </summary>
        /// <param name="filePath">Excelファイルパス</param>
        /// <param name="dt">DataTable</param>
        /// <param name="msgID">メッセージID</param>
        /// <param name="args">メッセージ引数</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/07/03</create>
        /// <create>J.Chen 2024/01/31 計画取込一括設定追加</create>
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

                // 担当者Code
                colName = Def_M_USER.USER_ID;
                colCaption = Resources.ExportUserMaster_StaffCode;
                expInfoCollection.Add(colName, colCaption);
                // 担当者名
                colName = Def_M_USER.USER_NAME;
                colCaption = Resources.ExportUserMaster_StaffName;
                expInfoCollection.Add(colName, colCaption);
                // MailAddress
                colName = Def_M_USER.MAIL_ADDRESS;
                colCaption = Resources.ExportUserMaster_MailAddress;
                expInfoCollection.Add(colName, colCaption);
                // MailAddress変更権限
                colName = ComDefine.FLD_MAIL_CHANGE_ROLE_NAME;
                colCaption = Resources.ExportUserMaster_ChangeAuthority;
                expInfoCollection.Add(colName, colCaption);
                // 荷姿表送信対象
                colName = ComDefine.FLD_MAIL_PACKING_FLAG_NAME;
                colCaption = Resources.ExportUserMaster_MailPackingFlag;
                expInfoCollection.Add(colName, colCaption);
                // TAG連携送信対象
                colName = ComDefine.FLD_MAIL_TAG_RENKEI_FLAG_NAME;
                colCaption = Resources.ExportUserMaster_MailTagRenkeiFlag;
                expInfoCollection.Add(colName, colCaption);
                // スタッフ区分
                colName = ComDefine.FLD_STAFF_KBN_NAME;
                colCaption = Resources.ExportUserMaster_StaffKbn;
                expInfoCollection.Add(colName, colCaption);
                // 計画取込一括設定
                colName = ComDefine.FLD_MAIL_SHUKKAKEIKAKU_FLAG_NAME;
                colCaption = Resources.ExportUserMaster_MailShukkakeikakuFlag;
                expInfoCollection.Add(colName, colCaption);
                // 権限
                colName = Def_M_ROLE.ROLE_NAME;
                colCaption = Resources.ExportUserMaster_Authority;
                expInfoCollection.Add(colName, colCaption);
                // 備考
                colName = Def_M_USER.USER_NOTE;
                colCaption = Resources.ExportUserMaster_Remarks;
                expInfoCollection.Add(colName, colCaption);

                var ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // 担当者Excelファイルを出力しました。
                    msgID = "M0100020025";
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
