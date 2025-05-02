using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices; // DLL Import

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
                //this.MakeCmbBox(this.cboSaiban, SAIBAN_FLAG.GROUPCD);
                //this.MakeCmbBox(this.cboListFlag, LIST_FLAG.GROUPCD);
                //string[] source = new string[] { FileType.ARGiren.ToString(), FileType.CaseMark.ToString() };
                //this.cboFileType.DataSource = source;
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

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool IsClipboardFormatAvailable(uint format);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetClipboardData(uint uFormat);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool CloseClipboard();


        private System.Drawing.Imaging.Metafile GetEnhMetafileOnClipboard(IntPtr hWnd) 
        {
            uint CF_ENHMETAFILE = 14;

		    System.Drawing.Imaging.Metafile meta = null;
		
            if (OpenClipboard(hWnd))
            {
                try 
	            {
                    if (IsClipboardFormatAvailable(CF_ENHMETAFILE) != false)
                    {
                        IntPtr hmeta = GetClipboardData(CF_ENHMETAFILE);
					    meta = new System.Drawing.Imaging.Metafile(hmeta, true);
				    }
	            }
	            catch (Exception ex)
	            {
            		System.Diagnostics.Debug.Print(ex.Message);
		            throw;
	            }
                finally
                {
                    CloseClipboard();
                }
				
            }
		
		    return meta;
        }


        private void dswFunctionButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rtf5.TextLength; i++)
            {
                rtf5.Select(i, 1);
                if (rtf5.SelectionType == RichTextBoxSelectionTypes.Object)
                {
                    // 画像部分のRTF形式取得
                    var rtf = rtf5.SelectedRtf;

                    var img = GetWmf2Rtf(rtf);

                    img.Save(@"C:\Users\055084.JOHO-OSAKA\Desktop\HexToBin10\result" + i.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                    img.Dispose();
                }
            }
        }

        private Image GetWmf2Rtf(string rtf)
        {
            string buff = rtf;


            do
            {
                // null または 空文字列で処理終了
                if (string.IsNullOrEmpty(buff)) return null;

                //M(42)B(4D)という文字を探す
                if (LeftEx(buff, 4).ToUpper() == "424D")
                {                 
                    break;
                }

                int sPos = buff.IndexOf("\r\n");

                // 見つからなかったら処理を抜ける
                if (sPos == -1) return null;

                buff = buff.Substring(sPos + 2, buff.Length - (sPos + 2));
            } while (true);

            // }{\resultという文字を探す
            int ePos = buff.LastIndexOf("\r\n"+@"}{\result");
            
            // 見つからなかったら処理を抜ける
            if (ePos == -1) return null;

            // 画像部分だけ抽出
            buff = buff.Substring(0, ePos);
            // 改行を削除
            buff = buff.Replace("\r\n", ""); 

            // バイト配列へ
            byte[] arr = Rtf2ByteArray(buff);

            // メモリストリームへ変換
            MemoryStream ms = new MemoryStream(arr);

            return System.Drawing.Image.FromStream(ms);
        }

        #region LEFT
        /// --------------------------------------------------
        /// <summary>
        /// 文字列の先頭から指定した長さの文字列を取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        /// <create>K.Tsutsumi 2019/06/30</create>
        /// <update></update>
        /// --------------------------------------------------    
        private string LeftEx(string str, int len)
        {
            if (len < 0)
            {
                return "";
            }
            if (str == null)
            {
                return "";
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(0, len);
        }
        #endregion

        #region Rtf2ByteArray

        private byte[] Rtf2ByteArray(string rtf)
        {

            byte[] result = new byte[rtf.Length / 2];
          

            int cur = 0;

            for (int i = 0; i < rtf.Length; i = i + 2)
            {
                string w = rtf.Substring(i, 2);
                result[cur] = byte.Parse(w, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
                cur++;
            }

            return result;
        }
        #endregion

    }
}
