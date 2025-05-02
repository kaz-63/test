using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace DSWControl.DSWRichTextBox
{
    /// --------------------------------------------------
    /// <summary>
    /// 画像情報
    /// </summary>
    /// <create>D.Okumura 2019/06/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public class DSWRichTextBoxImageItem
    {
        /// --------------------------------------------------
        /// <summary>
        /// 位置
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public int Position { get; set; }

        /// --------------------------------------------------
        /// <summary>
        /// 画像データであるかどうか
        /// </summary>
        /// <create>D.Okumura 2019/06/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsObject { get; internal set; }

        /// --------------------------------------------------
        /// <summary>
        /// RTFデータ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// <remarks>
        /// 設定時にオブジェクトデータであるかの判定を行う
        /// </remarks>
        /// --------------------------------------------------
        public string Rtf {
            set
            {
                this.CompressedData = TranslateImage(value);
                this.IsObject = GetRtfIsObject(ref value);
            }
            get { return TranslateImage(this.CompressedData); }
        }
        /// --------------------------------------------------
        /// <summary>
        /// 圧縮データ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// <remarks>
        /// 圧縮済みデータ。
        /// このメソッドではオブジェクトを含むデータであるかのチェックは行われないため、
        /// 手動で確認を行うこと。
        /// 必ずGZIP形式のデータを設定すること。
        /// 異なる場合、展開する段階でエラーとなる。
        /// 設定後、Validateメソッドによりチェックすること。
        /// </remarks>
        /// --------------------------------------------------
        public byte[] CompressedData { get; set; }


        /// --------------------------------------------------
        /// <summary>
        /// データのチェック
        /// </summary>
        /// <create>D.Okumura 2019/06/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void Validate()
        {
            var rtf = this.Rtf;
            this.IsObject = GetRtfIsObject(ref rtf);
        }

        /// --------------------------------------------------
        /// <summary>
        /// RTFがオブジェクトを含むかどうか
        /// </summary>
        /// <param name="rtf"></param>
        /// <returns></returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        internal static bool GetRtfIsObject(ref string rtf)
        {
            return rtf.Contains("{\\object");
        }

        /// --------------------------------------------------
        /// <summary>
        /// 変換処理
        /// </summary>
        /// <param name="data">入力データ</param>
        /// <returns>出力データ</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private static string TranslateImage(byte[] data)
        {
            using (MemoryStream input = new MemoryStream(data))
            using (GZipStream compress = new GZipStream(input, CompressionMode.Decompress))
            using (StreamReader reader = new StreamReader(compress, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// 変換処理
        /// </summary>
        /// <param name="rtf">入力データ</param>
        /// <returns>出力データ</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private static byte[] TranslateImage(string rtf)
        {
            using (MemoryStream output = new MemoryStream())
            {
                using (GZipStream compress = new GZipStream(output, CompressionMode.Compress))
                using (StreamWriter writer = new StreamWriter(compress, Encoding.UTF8))
                {
                    writer.Write(rtf);
                    writer.Flush();
                }
                return output.ToArray();
            }
        }
    }
}
