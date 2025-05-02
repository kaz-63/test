using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace DSWControl.DSWRichTextBox
{
    /// --------------------------------------------------
    /// <summary>
    /// リッチテキストボックス 貼り付けイベント情報
    /// </summary>
    /// <create>D.Okumura 2019/06/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public enum DSWRichTextBoxPasteEventStatus
    {
        /// --------------------------------------------------
        /// <summary>
        /// なし
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        None,

        /// --------------------------------------------------
        /// <summary>
        /// 成功
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        Success,
        /// --------------------------------------------------
        /// <summary>
        /// データなし
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        ErrorNoData,
        /// --------------------------------------------------
        /// <summary>
        /// 桁数オーバー
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        ErrorLengthOver,
        /// --------------------------------------------------
        /// <summary>
        /// 貼り付けサイズオーバー
        /// </summary>
        /// <create>D.Okumura 2019/06/21</create>
        /// <update></update>
        /// --------------------------------------------------
        ErrorSizeOver,
        /// --------------------------------------------------
        /// <summary>
        /// 画像ではないデータを含んでいる
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        WarnContainsNonImageData,
        /// --------------------------------------------------
        /// <summary>
        /// 画像ではないデータを含んでいる
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        ErrorContainsNonImageData,
        /// --------------------------------------------------
        /// <summary>
        /// 内部処理エラー：入力チェック文字にスペースが許容されていない
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        ErrorInvalidInputReguration,
        /// --------------------------------------------------
        /// <summary>
        /// 十分なメモリがありません
        /// </summary>
        /// <create>D.Okumura 2019/06/21</create>
        /// <update></update>
        /// --------------------------------------------------
        ErrorNoEnoughMemory,
        /// --------------------------------------------------
        /// <summary>
        /// その他エラー
        /// </summary>
        /// <create>D.Okumura 2019/06/21</create>
        /// <update></update>
        /// --------------------------------------------------
        Error,

    }
    /// --------------------------------------------------
    /// <summary>
    /// リッチテキストボックス 貼り付けイベント情報
    /// </summary>
    /// <create>D.Okumura 2019/06/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public class DSWRichTextBoxPasteEventArgs : EventArgs
    {
        #region プロパティ
        /// --------------------------------------------------
        /// <summary>
        /// 説明
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DSWRichTextBoxPasteEventStatus Status { get; private set; }

        #endregion //プロパティ
        /// --------------------------------------------------
        /// <summary>
        /// リッチテキストボックス 貼り付けイベント情報
        /// コンストラクタ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DSWRichTextBoxPasteEventArgs(DSWRichTextBoxPasteEventStatus status)
        {
            Status = status;
        }
    }
}
