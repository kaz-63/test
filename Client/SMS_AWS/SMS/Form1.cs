using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using SystemBase.Forms;
using SystemBase.Util;
using Commons;
using WsConnection;
using WsConnection.WebRefAttachFile;
using SMS.Properties;

namespace SMS
{
    public partial class Form1 : CustomOrderForm
    {
        public Form1(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }


        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            try
            {
                this.MakeCmbBox(this.cboSaiban, SAIBAN_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboListFlag, LIST_FLAG.GROUPCD);
                string[] source = new string[] { FileType.ARGiren.ToString(), FileType.CaseMark.ToString() };
                this.cboFileType.DataSource = source;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        private void btnComboTest1_Click(object sender, EventArgs e)
        {
            try
            {
                this.MakeCmbBox(this.cboComboTest1, this.txtComboTest1.Text);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        private void btnComboTest2_Click(object sender, EventArgs e)
        {
            try
            {
                this.MakeCmbBox(this.cboComboTest2, this.txtComboTest2.Text, true);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        private void btnMsgTest1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = this.ShowMessage(this.txtMsgTest1.Text);
                lblResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        private void btnMsgTest2_Click(object sender, EventArgs e)
        {
            try
            {
                this.ShowMessage(this.txtMsgTest2.Text);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            if (ofdImage.ShowDialog(this) == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(ofdImage.FileName, FileMode.Open, FileAccess.Read))
                using (Bitmap bmp = (Bitmap)Bitmap.FromStream(fs))
                {
                    picImage.Image = new Bitmap(bmp);
                    fs.Close();
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream(ofdImage.FileName, FileMode.Open, FileAccess.Read))
                using (System.IO.Stream strm = new System.IO.MemoryStream())
                {
                    picImage.Image.Save(strm, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ConnAttachFile conn = new ConnAttachFile();
                    FileUploadPackage package = new FileUploadPackage();

                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                    fs.Close();
                    strm.Write(data, 0, data.Length);
                    package.FileData = data;
                    package.FileName = this.txtImage.Text;
                    package.DeleteFileName = this.txtDelImage.Text;
                    if (this.cboFileType.SelectedValue.ToString() == FileType.ARGiren.ToString())
                    {
                        package.FileType = FileType.ARGiren;
                    }
                    else
                    {
                        package.FileType = FileType.CaseMark;
                    }
                    package.NonyusakiCD = this.txtNonyusakiCD.Text;
                    package.ARNo = this.txtARNo.Text;
                    package.GirenType = GirenType.No1;
                    package.KojiNo = this.txtKojiNo.Text;

                    FileUploadResult result = conn.FileUpload(package);
                    string msg = Resources.Form1_Failure;
                    if (result.IsSuccess)
                    {
                        msg = Resources.Form1_Success;
                    }
                    this.SetMessage(SystemBase.Controls.MessageImageType.Information, msg);

                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtImage.Text)) return;

            ConnAttachFile conn = new ConnAttachFile();
            FileDownloadPackage package = new FileDownloadPackage();

            package.FileName = this.txtImage.Text;
            if (this.cboFileType.SelectedValue.ToString() == FileType.ARGiren.ToString())
            {
                package.FileType = FileType.ARGiren;
            }
            else
            {
                package.FileType = FileType.CaseMark;
            }
            package.NonyusakiCD = this.txtNonyusakiCD.Text;
            package.ARNo = this.txtARNo.Text;
            package.GirenType = GirenType.No1;
            package.KojiNo = this.txtKojiNo.Text;

            FileDownloadResult result = conn.FileDownload(package);
            if (!result.IsExistsFile)
            {
                this.SetMessage(SystemBase.Controls.MessageImageType.Information, "ファイルがない");
                return;
            }

            using(System.IO.MemoryStream memStrm = new MemoryStream(result.FileData))
            using (Bitmap bmp = (Bitmap)Bitmap.FromStream(memStrm))
            {
                if (bmp == null) return;
                picImage.Image = new Bitmap(bmp);
            }
            //picImage.Image = new Bitmap(ARHelpFile.GetARList());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            picImage.Image = null;
        }

        private void btnSaiban_Click(object sender, EventArgs e)
        {
            try
            {
                WsConnection.WebRefSms.CondSms cond = new WsConnection.WebRefSms.CondSms(this.UserInfo);
                WsConnection.ConnSms conn = new WsConnection.ConnSms();

                cond.SaibanFlag = this.cboSaiban.SelectedValue.ToString();
                cond.ARUS = this.txtARUS.Text;
                cond.ListFlag = this.cboListFlag.SelectedValue.ToString();
                string saiban = string.Empty;
                string msgID = string.Empty;
                if (conn.GetSaiban(cond, out saiban, out msgID))
                {
                    this.SetMessage(SystemBase.Controls.MessageImageType.None, saiban);
                }
                else
                {
                    this.ShowMessage(msgID);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }
    }
}
