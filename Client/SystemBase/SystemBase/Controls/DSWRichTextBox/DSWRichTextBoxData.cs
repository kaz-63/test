using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
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
    internal class DSWRichTextBoxData
    {
        /// --------------------------------------------------
        /// <summary>
        /// 文字列
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Text { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// データ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public List<DSWRichTextBoxImageItem> Images { get; private set; }


        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DSWRichTextBoxData()
        {
            Images = new List<DSWRichTextBoxImageItem>();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック等により削減されたデータを取得します
        /// </summary>
        /// <param name="inputText">入力チェック後の文字列</param>
        /// <returns>再配置結果</returns>
        /// <create>D.Okumura 2019/06/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public DSWRichTextBoxData ShrinkText(string inputText)
        {
            DSWRichTextBoxData newData = new DSWRichTextBoxData();
            int dataOffset = 0;
            int inputOffset = 0;
            int imageOffset = 0;
            int factorNewLine = 0;
            //System.Diagnostics.Debug.WriteLine(string.Format("paste: {0} => {1} images:{2}", Text.Length, inputText.Length, Images.Count));
            //System.Diagnostics.Debug.WriteLine(Text);
            while (dataOffset < Text.Length && inputOffset < inputText.Length && imageOffset < Images.Count)
            {
                // 改行はスキップして両者の値を取得する
                char check1;
                do
                {
                    check1 = Text[dataOffset];
                    if (check1 != '\r' && check1 != '\n')
                        break;
                    dataOffset++;
                } while (dataOffset < Text.Length);
                char check2;
                do
                {
                    check2 = inputText[inputOffset];
                    if (check2 == '\n')
                        factorNewLine++;
                    if (check2 != '\r' && check2 != '\n')
                        break;
                    inputOffset++;
                } while (inputOffset < inputText.Length);

                if (check1 == check2)
                {

                    if (Images[imageOffset].Position == dataOffset)
                    {
                        newData.Images.Add(new DSWRichTextBoxImageItem()
                        {
                            CompressedData = Images[imageOffset].CompressedData,
                            IsObject = Images[imageOffset].IsObject,
                            Position = inputOffset - factorNewLine,
                        });
                        //System.Diagnostics.Debug.WriteLine(string.Format("paste: [{0}]{1} => {2}", imageOffset, dataOffset, inputOffset - factorNewLine));
                        imageOffset++;
                    }
                    inputOffset++;
                }
                else if (Images[imageOffset].Position < dataOffset)
                {
                    //lost
                    //System.Diagnostics.Debug.WriteLine(string.Format("lost: {0}", imageOffset));
                    imageOffset++;
                }
                dataOffset++;
            }
            newData.Text = inputText;
            return newData;
        }
    }
}
