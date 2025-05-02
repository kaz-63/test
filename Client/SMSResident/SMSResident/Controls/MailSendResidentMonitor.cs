using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using Commons;
using WsConnection;
using WsConnection.WebRefJ01;
using System.Net.NetworkInformation;
using System.Net.Mail;

namespace SMSResident.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// メール送信処理用コントロール
    /// </summary>
    /// <create>R.Katsuo 2017/09/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class MailSendResidentMonitor : SMSResident.Controls.BaseResidentMonitor
    {
        #region Fields

        #endregion

        #region メッセージ定数

        #endregion

        #region Delegate

        /// --------------------------------------------------
        /// <summary>
        /// 前回締め処理時間設定用デリゲート
        /// </summary>
        /// <param name="date"></param>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void SetShimeDateTextDelegate(DateTime date);

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public MailSendResidentMonitor()
            : base()
        {
            InitializeComponent();
        }

        #endregion

        #region 締め処理時間設定

        /// --------------------------------------------------
        /// <summary>
        /// 締め処理時間設定
        /// </summary>
        /// <param name="date">前回処理時間</param>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetShimeDateText(DateTime date)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetShimeDateTextDelegate(this.SetShimeDateText), new Object[] { date });
                return;
            }
            this.txtNichijiDate.Text = date.ToString("yyyy/MM/dd HH:mm:ss");
        }

        #endregion

        #region 常駐処理関係

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理呼出しスレッド
        /// </summary>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void AsynchronousMonitor()
        {
            try
            {
                // 常駐処理を始める前の初期処理をここに記述します。
                // InitializeControlがOnLoadで呼ばれるためタブが表示されていないと実行されないので処理に必要な分は此処で行う。
                var dtMailSetting = this.GetMailSetting();
                this.SetCheckTime(dtMailSetting);

                // 初期処理で常駐処理を抜ける場合は下記の様にして下さい。
                //this.IsStopRequest = true;
                //base.AsynchronousMonitor();
                //return;

                // 常駐処理開始
                base.AsynchronousMonitor();
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理
        /// </summary>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void MonitorExecute()
        {
            var nowDate = DateTime.Now;
            try
            {
                var dtMailSetting = this.GetMailSetting();
                this.SetCheckTime(dtMailSetting);

                // PING実行
                if (!this.ExecPing(dtMailSetting))
                {
                    this.MessageAdd("PING失敗：" + UtilData.GetFld(dtMailSetting, 0, Def_M_MAIL_SETTING.SMTP_SERVER));
                    return;
                }

                // 対象データ取得
                var conn = new ConnJ01();
                var ds = conn.GetMail();
                if (ds == null
                    || ds.Tables.Count == 0
                    || ds.Tables[Def_T_MAIL.Name].Rows.Count == 0)
                {
                    return;
                }

                // メール送信処理
                foreach (DataRow dr in ds.Tables[Def_T_MAIL.Name].Rows)
                {
                    this.RunMail(dtMailSetting, dr);
                }
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
            }
            finally
            {
                this.SetShimeDateText(nowDate);
            }
        }

        #endregion

        #region メール送信間隔を設定

        /// --------------------------------------------------
        /// <summary>
        /// メール設定マスタ取得
        /// </summary>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetMailSetting()
        {
            var conn = new ConnJ01();
            var ds = conn.GetMailSetting();
            return ds.Tables[Def_M_MAIL_SETTING.Name];
        }

        /// --------------------------------------------------
        /// <summary>
        /// メール送信間隔を設定
        /// </summary>
        /// <param name="dt">メール設定マスタ</param>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetCheckTime(DataTable dt)
        {
            this.CheckTime = ComFunc.GetFldToInt32(dt, 0, Def_M_MAIL_SETTING.MAIL_SPAN, 30);
        }

        #endregion

        #region PINGの実行

        /// --------------------------------------------------
        /// <summary>
        /// PINGの実行
        /// </summary>
        /// <param name="dtMailSetting">メール設定マスタ</param>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExecPing(DataTable dtMailSetting)
        {
            int retry = 4;
            int sleep = 1000 * 5;

            try
            {
                using (var ping = new Ping())
                {
                    for (int i = 1; i <= retry; i++)
                    {
                        if (i != 1)
                        {
                            // 失敗している場合、数秒待ってからリトライ
                            System.Threading.Thread.Sleep(sleep);
                        }

                        // PING送信
                        var reply = ping.Send(UtilData.GetFld(dtMailSetting, 0, Def_M_MAIL_SETTING.SMTP_SERVER));

                        // 結果を取得
                        if (reply.Status == IPStatus.Success)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region メール送信制御

        /// --------------------------------------------------
        /// <summary>
        /// メール送信制御
        /// </summary>
        /// <param name="dtMailSetting">メール設定マスタ</param>
        /// <param name="dr">メールデータ</param>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update>H.Tajimi 2018/11/28 添付メール対応</update>
        /// --------------------------------------------------
        private void RunMail(DataTable dtMailSetting, DataRow dr)
        {
            string errMsg = string.Empty;
            string dispMsg = string.Empty;
            int retry = UtilData.GetFldToInt32(dr, Def_T_MAIL.RETRY_COUNT);
            var filePath = ComFunc.GetFld(dr, Def_T_MAIL.APPENDIX_FILE_PATH);
            var lang = ComFunc.GetFld(dr, Def_T_MAIL.LANG);
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (!File.Exists(filePath))
                    {
                        // 添付対象となるファイルが存在しない
                        retry++;
                        // ファイルの添付に失敗しました。
                        errMsg = "ファイルの添付に失敗";
                        dispMsg = SEND_MAIL_FAILURE_REASON.ATTACHMENT_VALUE1;
                        this.MessageAdd("メール送信失敗。ID：" + UtilData.GetFld(dr, Def_T_MAIL.MAIL_ID) + "、エラー：" + "ファイルの添付に失敗");
                        return;
                    }
                }
                this.SendMail(dtMailSetting, dr);
                this.MessageAdd("メール送信完了。ID：" + UtilData.GetFld(dr, Def_T_MAIL.MAIL_ID));
            }
            catch (Exception ex)
            {
                retry++;
                errMsg = ex.Message;
                dispMsg = SEND_MAIL_FAILURE_REASON.GENERAL_VALUE1;
                this.MessageAdd("メール送信失敗。ID：" + UtilData.GetFld(dr, Def_T_MAIL.MAIL_ID) + "、エラー：" + ex.Message);
            }
            finally
            {
                this.UpdMail(dtMailSetting, dr, errMsg, dispMsg, retry);
            }
        }

        #endregion

        #region メール送信

        /// --------------------------------------------------
        /// <summary>
        /// メール送信
        /// </summary>
        /// <param name="dtMailSetting">メール設定マスタ</param>
        /// <param name="dr">メールデータ</param>
        /// <return></return>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update>H.Tajimi 2018/11/27 添付メール対応</update>
        /// --------------------------------------------------
        private void SendMail(DataTable dtMailSetting, DataRow dr)
        {
            using (var mail = new MailMessage())
            {
                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.Normal;
                mail.SubjectEncoding = System.Text.UTF8Encoding.UTF8;
                mail.BodyEncoding = System.Text.UTF8Encoding.UTF8;
                
                mail.Subject = UtilData.GetFld(dr, Def_T_MAIL.TITLE).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);
                mail.Body = UtilData.GetFld(dr, Def_T_MAIL.NAIYO);
                
                mail.From = new MailAddress(UtilData.GetFld(dr, Def_T_MAIL.MAIL_SEND), UtilData.GetFld(dr, Def_T_MAIL.MAIL_SEND_DISPLAY));
                foreach (var item in this.GetMailAddresses(UtilData.GetFld(dr, Def_T_MAIL.MAIL_TO), UtilData.GetFld(dr, Def_T_MAIL.MAIL_TO_DISPLAY)))
                {
                    mail.To.Add(item);
                }
                foreach (var item in this.GetMailAddresses(UtilData.GetFld(dr, Def_T_MAIL.MAIL_CC), UtilData.GetFld(dr, Def_T_MAIL.MAIL_CC_DISPLAY)))
                {
                    mail.CC.Add(item);
                }
                foreach (var item in this.GetMailAddresses(UtilData.GetFld(dr, Def_T_MAIL.MAIL_BCC), UtilData.GetFld(dr, Def_T_MAIL.MAIL_BCC_DISPLAY)))
                {
                    mail.Bcc.Add(item);
                }

                // 添付メールを添付
                var filePath = ComFunc.GetFld(dr, Def_T_MAIL.APPENDIX_FILE_PATH);
                if (!string.IsNullOrEmpty(filePath))
                {
                    var attach = new Attachment(filePath);
                    mail.Attachments.Add(attach);
                }

                var smtp = new SmtpClient();
                smtp.Host = UtilData.GetFld(dtMailSetting, 0, Def_M_MAIL_SETTING.SMTP_SERVER);
                smtp.Port = UtilData.GetFldToInt32(dtMailSetting, 0, Def_M_MAIL_SETTING.SMTP_PORT);
                //smtp.Timeout = 30;
                smtp.Send(mail);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メールアドレス取得
        /// </summary>
        /// <param name="addresses">メールアドレス</param>
        /// <param name="displayNames">表示名</param>
        /// <returns></returns>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private List<MailAddress> GetMailAddresses(string addresses, string displayNames)
        {
            var lst = new List<MailAddress>();
            var sptAddress = addresses.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var sptName = displayNames.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < sptAddress.Length; i++)
            {
                if (sptAddress.Length == sptName.Length)
                {
                    lst.Add(new MailAddress(sptAddress[i], sptName[i]));
                }
                else
                {
                    lst.Add(new MailAddress(sptAddress[i]));
                }
            }
            return lst;
        }

        #endregion

        #region メール送信後DB更新

        /// --------------------------------------------------
        /// <summary>
        /// メール送信後DB更新
        /// </summary>
        /// <param name="dtMailSetting">メール設定マスタ</param>
        /// <param name="dr">メールデータ</param>
        /// <param name="errMsg">エラー時メッセージ</param>
        /// <param name="dispMsg">表示用エラーメッセージ</param>
        /// <param name="retry">リトライ数</param>
        /// <create>R.Katsuo 2017/09/07</create>
        /// <update>H.Tajimi 2018/11/28 添付メール対応</update>
        /// --------------------------------------------------
        private void UpdMail(DataTable dtMailSetting, DataRow dr, string errMsg, string dispMsg, int retry)
        {
            try
            {
                var dt = dr.Table.Clone();
                dt.Rows.Add(dr.ItemArray);
                UtilData.SetFld(dt, 0, Def_T_MAIL.REASON, errMsg);
                UtilData.SetFld(dt, 0, Def_T_MAIL.DISP_REASON, dispMsg);

                if (string.IsNullOrEmpty(errMsg))
                {
                    UtilData.SetFld(dt, 0, Def_T_MAIL.MAIL_STATUS, MAIL_STATUS.ZUMI_VALUE1);
                }
                else
                {
                    UtilData.SetFld(dt, 0, Def_T_MAIL.RETRY_COUNT, retry);
                    if (UtilData.GetFldToInt32(dtMailSetting, 0, Def_M_MAIL_SETTING.MAIL_RETRY) <= retry)
                    {
                        UtilData.SetFld(dt, 0, Def_T_MAIL.MAIL_STATUS, MAIL_STATUS.ERROR_VALUE1);
                    }
                }

                var conn = new ConnJ01();
                conn.UpdMail(dt);
            }
            catch { }
        }

        #endregion

    }
}
