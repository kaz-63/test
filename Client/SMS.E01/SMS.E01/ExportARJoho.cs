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
    /// AR情報Excel出力クラス
    /// </summary>
    /// <create>M.Tsutsumi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportARJoho : BaseExport
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExportARJoho()
            : base()
        {
        }

        #endregion

        #region AR情報出力

        /// --------------------------------------------------
        /// <summary>
        /// AR情報Excel出力
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/27</create>
        /// <update>K.Tsutsumi 2011/02/18 No32</update>
        /// <update>D.Okumura 2019/06/18 添付ファイル追加対応(技連4-5)</update>
        /// <update>T.Nukaga 2019/11/20 AR7000番運用対応</update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, DataTable dt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                string colName = string.Empty;
                string colCaption = string.Empty;
                string dateTimeFormat = "yyyy/MM/dd";
                ExportInfoCollection expInfoCollection = new ExportInfoCollection();

                // 納入先
                colName = Def_M_NONYUSAKI.NONYUSAKI_NAME;
                colCaption = Resources.ExportARJoho_DeliveryDestination;
                expInfoCollection.Add(colName, colCaption);
                // リスト
                colName = "LIST_FLAG_NAME";
                // 2011/02/18 K.Tsutsumi Change No32
                //colCaption = "リスト";
                colCaption = Resources.ExportARJoho_List;
                // ↑
                expInfoCollection.Add(colName, colCaption);
                // ARNO
                colName = Def_T_AR.AR_NO;
                colCaption = Resources.ExportARJoho_ARNo;
                expInfoCollection.Add(colName, colCaption);
                // 状況
                colName = "JYOKYO_FLAG_NAME";
                colCaption = Resources.ExportARJoho_Situation;
                expInfoCollection.Add(colName, colCaption);
                // 更新日時
                colName = Def_T_AR.UPDATE_DATE;
                colCaption = Resources.ExportARJoho_DateModified;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 更新者
                colName = Def_T_AR.UPDATE_USER_NAME;
                colCaption = Resources.ExportARJoho_ModifiedBy;
                expInfoCollection.Add(colName, colCaption);
                // 発生日時
                colName = Def_T_AR.HASSEI_DATE;
                colCaption = Resources.ExportARJoho_TimeAndDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 連絡者
                colName = Def_T_AR.RENRAKUSHA;
                colCaption = Resources.ExportARJoho_Connector;
                expInfoCollection.Add(colName, colCaption);
                // 機種
                colName = Def_T_AR.KISHU;
                colCaption = Resources.ExportARJoho_Model;
                expInfoCollection.Add(colName, colCaption);
                // 号機
                colName = Def_T_AR.GOKI;
                colCaption = Resources.ExportARJoho_Unit;
                expInfoCollection.Add(colName, colCaption);
                // 現場到着希望日
                colName = Def_T_AR.GENBA_TOTYAKUKIBOU_DATE;
                colCaption = Resources.ExportARJoho_SiteDesiredArrivalDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 不具合内容
                colName = "HUGUAI_ALL";
                colCaption = Resources.ExportARJoho_DefectContent;
                expInfoCollection.Add(colName, colCaption);
                // 対策内容
                colName = "TAISAKU_ALL";
                colCaption = Resources.ExportARJoho_Countermeasures;
                expInfoCollection.Add(colName, colCaption);
                // 対応部署
                colName = Def_T_AR.TAIO_BUSHO;
                colCaption = Resources.ExportARJoho_CorrespondingDepartment;
                expInfoCollection.Add(colName, colCaption);
                // 技連No①
                colName = Def_T_AR.GIREN_NO_1;
                colCaption = Resources.ExportARJoho_GirenNo1;
                expInfoCollection.Add(colName, colCaption);
                // 技連No②
                colName = Def_T_AR.GIREN_NO_2;
                colCaption = Resources.ExportARJoho_GirenNo2;
                expInfoCollection.Add(colName, colCaption);
                // 技連No③
                colName = Def_T_AR.GIREN_NO_3;
                colCaption = Resources.ExportARJoho_GirenNo3;
                expInfoCollection.Add(colName, colCaption);
                // 現地・手配先
                colName = Def_T_AR.GENCHI_TEHAISAKI;
                colCaption = Resources.ExportARJoho_LocalArrangementsDestination;
                expInfoCollection.Add(colName, colCaption);
                // 現地・設定納期
                colName = Def_T_AR.GENCHI_SETTEINOKI_DATE;
                colCaption = Resources.ExportARJoho_LocalSettingDeliveryTime;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 現地・出荷予定日
                colName = Def_T_AR.GENCHI_SHUKKAYOTEI_DATE;
                colCaption = Resources.ExportARJoho_LocalShippingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 現地・工場出荷日
                colName = Def_T_AR.GENCHI_KOJYOSHUKKA_DATE;
                colCaption = Resources.ExportARJoho_LocalFactoryDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 出荷方法
                colName = Def_T_AR.SHUKKAHOHO;
                colCaption = Resources.ExportARJoho_ShippingMethod;
                expInfoCollection.Add(colName, colCaption);
                // 日本・設定納期
                colName = Def_T_AR.JP_SETTEINOKI_DATE;
                colCaption = Resources.ExportARJoho_JpSettingDeliveryTime;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 日本・出荷予定日
                colName = Def_T_AR.JP_SHUKKAYOTEI_DATE;
                colCaption = Resources.ExportARJoho_JpShippingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 日本・工場出荷日
                colName = Def_T_AR.JP_KOJYOSHUKKA_DATE;
                colCaption = Resources.ExportARJoho_JpFactoryShippingDate;
                expInfoCollection.Add(colName, colCaption, dateTimeFormat);
                // 日本・運送会社
                colName = Def_T_AR.JP_UNSOKAISHA_NAME;
                colCaption = Resources.ExportARJoho_JpShippingCompany;
                expInfoCollection.Add(colName, colCaption);
                // 日本・送り状No
                colName = Def_T_AR.JP_OKURIJYO_NO;
                colCaption = Resources.ExportARJoho_JpInvoiceNo;
                expInfoCollection.Add(colName, colCaption);
                // GMS発行No
                colName = Def_T_AR.GMS_HAKKO_NO;
                colCaption = Resources.ExportARJoho_GMSIssueNo;
                expInfoCollection.Add(colName, colCaption);
                // 仕様連絡No
                colName = Def_T_AR.SHIYORENRAKU_NO;
                colCaption = Resources.ExportARJoho_SpecificationsContactNo;
                expInfoCollection.Add(colName, colCaption);
                // 備考
                colName = Resources.ExportARJoho_BIKO_ALL;
                colCaption = Resources.ExportARJoho_Remarks;
                expInfoCollection.Add(colName, colCaption);
                // 発生原因
                colName = Def_T_AR.HASSEI_YOUIN;
                colCaption = Resources.ExportAllARJoho_HasseiYouin;
                expInfoCollection.Add(colName, colCaption);
                // 参考資料No1
                colName = Def_T_AR.REFERENCE_NO_1;
                colCaption = Resources.ExportAllARJoho_ReferenceNo1;
                expInfoCollection.Add(colName, colCaption);
                // 参考資料No2
                colName = Def_T_AR.REFERENCE_NO_2;
                colCaption = Resources.ExportAllARJoho_ReferenceNo2;
                expInfoCollection.Add(colName, colCaption);
                // 技連No④
                colName = Def_T_AR.GIREN_NO_4;
                colCaption = Resources.ExportARJoho_GirenNo4;
                expInfoCollection.Add(colName, colCaption);
                // 技連No⑤
                colName = Def_T_AR.GIREN_NO_5;
                colCaption = Resources.ExportARJoho_GirenNo5;
                expInfoCollection.Add(colName, colCaption);
                // 元ARNo
                colName = Def_T_AR.MOTO_AR_NO;
                colCaption = Resources.ExportARJoho_MotoARNo;
                expInfoCollection.Add(colName, colCaption);
                // 結果ARNo
                colName = ComDefine.FLD_KEKKA_AR_NO;
                colCaption = Resources.ExportARJoho_KekkaARNo;
                expInfoCollection.Add(colName, colCaption);

                bool ret = this.ExportExcel(filePath, dt, expInfoCollection, out msgID, out args);
                if (ret)
                {
                    // AR情報Excelファイルを出力しました。
                    msgID = "A0100010005";
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
