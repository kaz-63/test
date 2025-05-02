using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemBase.Excel;
using Commons;
using System.Data;
using SMS.E01.Properties;

namespace SMS.E01
{
    /// --------------------------------------------------
    /// <summary>
    /// 全体AR情報Excel出力クラス
    /// </summary>
    /// <create>T.Sakiori 2012/04/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ExportAllARJoho : BaseExport
    {
        #region 全体AR情報出力

        /// --------------------------------------------------
        /// <summary>
        /// 全体AR情報出力
        /// </summary>
        /// <param name="filePath">出力先パス</param>
        /// <param name="dt">出力データ</param>
        /// <param name="msgID">エラーメッセージ</param>
        /// <param name="args">パラメータ</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/26</create>
        /// <update>H.Tajimi 2018/10/19 FE要望対応</update>
        /// <update>D.Okumura 2019/06/21 添付ファイル追加対応(技連4-5)</update>
        /// <update>T.Nukaga 2019/11/20 AR7000番運用対応</update>
        /// --------------------------------------------------
        public bool ExportExcel(string filePath, DataTable dt, out string msgID, out string[] args)
        {
            msgID = string.Empty;
            args = null;
            try
            {
                var info = new ExportInfoCollection();
                
                // 納入先
                info.Add(Def_M_BUKKEN.BUKKEN_NAME, Resources.ExportAllARJoho_DeliveryDestination);
                // リスト
                info.Add(Def_T_AR.LIST_FLAG, Resources.ExportAllARJoho_List);
                // ARNO
                info.Add(Def_T_AR.AR_NO, Resources.ExportAllARJoho_ARNo);
                // 状況
                info.Add(Def_T_AR.JYOKYO_FLAG, Resources.ExportAllARJoho_Situation);
                // 更新日時
                info.Add(Def_T_AR.UPDATE_DATE, Resources.ExportAllARJoho_DateModified);
                // 更新者
                info.Add(Def_T_AR.UPDATE_USER_NAME, Resources.ExportAllARJoho_ModifiedBy);
                // 発生日時
                info.Add(Def_T_AR.HASSEI_DATE, Resources.ExportAllARJoho_TimeAndDate);
                // 連絡者
                info.Add(Def_T_AR.RENRAKUSHA, Resources.ExportAllARJoho_Connector);
                // 機種
                info.Add(Def_T_AR.KISHU, Resources.ExportAllARJoho_Model);
                // 号機
                info.Add(Def_T_AR.GOKI, Resources.ExportAllARJoho_Unit);
                // 現場到着希望日
                info.Add(Def_T_AR.GENBA_TOTYAKUKIBOU_DATE, Resources.ExportAllARJoho_SiteDesiredArrivalDate);
                // 不具合内容
                info.Add(Def_T_AR.HUGUAI, Resources.ExportAllARJoho_DefectContent);
                // 対策内容
                info.Add(Def_T_AR.TAISAKU, Resources.ExportAllARJoho_Countermeasures);
                // 対応部署
                info.Add(Def_T_AR.TAIO_BUSHO, Resources.ExportAllARJoho_CorrespondingDepartment);
                // 技連No1
                info.Add(Def_T_AR.GIREN_NO_1, Resources.ExportAllARJoho_GirenNo1);
                // 技連No2
                info.Add(Def_T_AR.GIREN_NO_2, Resources.ExportAllARJoho_GirenNo2);
                // 技連No3
                info.Add(Def_T_AR.GIREN_NO_3, Resources.ExportAllARJoho_GirenNo3);
                // 現地・手配先
                info.Add(Def_T_AR.GENCHI_TEHAISAKI, Resources.ExportAllARJoho_LocalArrangementsDestination);
                // 現地・設定納期
                info.Add(Def_T_AR.GENCHI_SETTEINOKI_DATE, Resources.ExportAllARJoho_LocalSettingDeliveryTime);
                // 現地・出荷予定日
                info.Add(Def_T_AR.GENCHI_SHUKKAYOTEI_DATE, Resources.ExportAllARJoho_LocalShippingDate);
                // 現地・工場出荷日
                info.Add(Def_T_AR.GENCHI_KOJYOSHUKKA_DATE, Resources.ExportAllARJoho_LocalFactoryDate);
                // 出荷方法
                info.Add(Def_T_AR.SHUKKAHOHO, Resources.ExportAllARJoho_ShippingMethod);
                // 日本・設定納期
                info.Add(Def_T_AR.JP_SETTEINOKI_DATE, Resources.ExportAllARJoho_JpSettingDeliveryTime);
                // 日本・出荷予定日
                info.Add(Def_T_AR.JP_SHUKKAYOTEI_DATE, Resources.ExportAllARJoho_JpShippingDate);
                // 日本・工場出荷日
                info.Add(Def_T_AR.JP_KOJYOSHUKKA_DATE, Resources.ExportAllARJoho_JpFactoryDate);
                // 日本・運送会社
                info.Add(Def_T_AR.JP_UNSOKAISHA_NAME, Resources.ExportAllARJoho_JpShippingCompany);
                // 日本・送り状No
                info.Add(Def_T_AR.JP_OKURIJYO_NO, Resources.ExportAllARJoho_JpInvoiceNumber);
                // GMS発行No
                info.Add(Def_T_AR.GMS_HAKKO_NO, Resources.ExportAllARJoho_GmsIssueNumber);
                // 仕様連絡No
                info.Add(Def_T_AR.SHIYORENRAKU_NO, Resources.ExportAllARJoho_SpecificationCommunicationNo);
                // 備考
                info.Add(Def_T_AR.BIKO, Resources.ExportAllARJoho_Remarks);
                // 発生原因
                info.Add(Def_T_AR.HASSEI_YOUIN, Resources.ExportAllARJoho_HasseiYouin);
                // 参考資料No1
                info.Add(Def_T_AR.REFERENCE_NO_1, Resources.ExportAllARJoho_ReferenceNo1);
                // 参考資料No2
                info.Add(Def_T_AR.REFERENCE_NO_2, Resources.ExportAllARJoho_ReferenceNo2);
                // 技連No4
                info.Add(Def_T_AR.GIREN_NO_4, Resources.ExportAllARJoho_GirenNo4);
                // 技連No5
                info.Add(Def_T_AR.GIREN_NO_5, Resources.ExportAllARJoho_GirenNo5);
                // 元ARNo
                info.Add(Def_T_AR.MOTO_AR_NO, Resources.ExportAllARJoho_MotoARNo);
                // 結果ARNo
                info.Add(ComDefine.FLD_KEKKA_AR_NO, Resources.ExportAllARJoho_KekkaARNo);

                bool ret = this.ExportExcel(filePath, dt, info, out msgID, out args);
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
