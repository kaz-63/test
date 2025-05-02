using System;
using System.Data;
using Commons;
using System.Text;
using System.Drawing;
using DataDynamics.ActiveReports.Document;

namespace SMS.R01
{
    /// <summary>
    /// 検品リスト の概要の説明です。
    /// </summary>
    public partial class RepTehaiKenpinList : DataDynamics.ActiveReports.ActiveReport
    {
        private int detailCount = 0;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// ActiveReport デザイナ サポートに必要です。
        /// </remarks>
        public RepTehaiKenpinList()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 開始イベント
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        private void RepKenpinList_ReportStart(object sender, EventArgs e)
        {
            this.lblDate.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        }
        /// <summary>
        /// ヘッダーフォーマットイベント
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        private void pageHeader_Format(object sender, EventArgs e)
        {
            lblNohinsaki.Text = lblNohinSakiName.Text + lblNohinSakiData.Text;
        }
        /// <summary>
        /// 納品先グループ終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 納品先が切り替わった後で、連番を0にする
        /// 各グループのVisibleはtrueにしておくこと(イベントが発生しなくなる)
        /// </remarks>
        private void groupFooterNohinsaki_AfterPrint(object sender, EventArgs e)
        {
            //切り替わったタイミングで連番をリセット
            detailCount = 0;

        }
        /// <summary>
        /// 詳細行イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 計算が必要なフィールドについて表示設定を行う
        /// 空行であってもコールされる
        /// </remarks>
        private void detail_Format(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTehaiNo.Text))
            {
                txtNo.Value = null;
                barcode.Text = "";
                txtSeibanCode.Text = "";
                return;
            }
            detailCount++;
            // 手配区分（発注）以外の場合は"2"で出力される
            var isPcOnly = (txtPcOnlyData.Text == "1" || txtPcOnlyData.Text == "2") ? true : false;

            if (txtPcOnlyData.Text == "2")
            {
                txtTehaiNo.Value = null;
            }

            txtNo.Value = detailCount;
            txtPcOnly.Visible = isPcOnly;
            barcode.Visible = !isPcOnly;
            //コントロールに設定する値はアスタリスクを付与しない（自動付与）
            barcode.Text = string.IsNullOrEmpty(txtTehaiNo.Text) ? "" : string.Format("{0}00", txtTehaiNo.Text);
            //製番-コードに整形する
            txtSeibanCode.Text = (string.IsNullOrEmpty(txtSeiban.Text) && string.IsNullOrEmpty(txtCode.Text)) 
                ? "" : string.Format("{0}-{1}", txtSeiban.Text, txtCode.Text);



            // 仮のPageを生成します。
            Page MeasurePage = new Page();


            double dFontSize_EcsNo = 9.75;
            double dFontSize_Keishiki = 9.75;
            double dFontSize_Hinmeijp = 9.75;
            double dFontSize_Hinmei = 9.75;

            // Ecs No
            if (!string.IsNullOrEmpty(this.txtEcsNo.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtEcsNo.Text);
                if (byteOfString.Length > 18)
                {
                    dFontSize_EcsNo = 6;
                }
                else if (byteOfString.Length > 15)
                {
                    dFontSize_EcsNo = 6.75;
                }
                else if (byteOfString.Length > 13)
                {
                    dFontSize_EcsNo = 8.25;
                }
                else if (byteOfString.Length > 12)
                {
                    dFontSize_EcsNo = 9;
                }

                float fontSize = (float)dFontSize_EcsNo;
                Font ft = new Font(this.txtEcsNo.Font.Name, fontSize);
                this.txtEcsNo.Font = ft;
            }

            // 形式
            if (!string.IsNullOrEmpty(this.txtZumenKeishiki.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtZumenKeishiki.Text);
                if (byteOfString.Length > 74)
                {
                    dFontSize_Keishiki = 3.75;
                }
                else if (byteOfString.Length > 64)
                {
                    dFontSize_Keishiki = 5.25;
                }
                else if (byteOfString.Length > 58)
                {
                    dFontSize_Keishiki = 6;
                }
                else if (byteOfString.Length > 47)
                {
                    dFontSize_Keishiki = 6.75;
                }
                else if (byteOfString.Length > 42)
                {
                    dFontSize_Keishiki = 8.25;
                }
                else if (byteOfString.Length > 40)
                {
                    dFontSize_Keishiki = 9;
                }

                float fontSize = (float)dFontSize_Keishiki;
                Font ft = new Font(this.txtZumenKeishiki.Font.Name, fontSize);
                this.txtZumenKeishiki.Font = ft;
            }

            // 品名
            if (!string.IsNullOrEmpty(this.txtHinmei.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtHinmei.Text);
                if (byteOfString.Length > 78)
                {
                    dFontSize_Hinmei = 3.75;
                }
                else if (byteOfString.Length > 68)
                {
                    dFontSize_Hinmei = 5.25;
                }
                else if (byteOfString.Length > 60)
                {
                    dFontSize_Hinmei = 6;
                }
                else if (byteOfString.Length > 50)
                {
                    dFontSize_Hinmei = 6.75;
                }
                else if (byteOfString.Length > 46)
                {
                    dFontSize_Hinmei = 8.25;
                }
                else if (byteOfString.Length > 42)
                {
                    dFontSize_Hinmei = 9;
                }

                float fontSize = (float)dFontSize_Hinmei;
                Font ft = new Font(this.txtHinmei.Font.Name, fontSize);
                this.txtHinmei.Font = ft;
            }

            // 品名（和文）
            if (!string.IsNullOrEmpty(this.txtHinmeiJ.Text))
            {
                byte[] byteOfString = System.Text.Encoding.Default.GetBytes(this.txtHinmeiJ.Text);
                if (byteOfString.Length > 94)
                {
                    dFontSize_Hinmeijp = 5.25;
                } 
                else if (byteOfString.Length > 84)
                {
                    dFontSize_Hinmeijp = 6;
                }
                else if (byteOfString.Length > 68)
                {
                    dFontSize_Hinmeijp = 6.75;
                }
                else if (byteOfString.Length > 62)
                {
                    dFontSize_Hinmeijp = 8.25;
                }
                else if (byteOfString.Length > 58)
                {
                    dFontSize_Hinmeijp = 9;
                }

                float fontSize = (float)dFontSize_Hinmeijp;
                Font ft = new Font(this.txtHinmeiJ.Font.Name, fontSize);
                this.txtHinmeiJ.Font = ft;
            }
        }


    }
}
