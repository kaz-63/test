using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DSWControl.DSWRichTextBox;
using DSWUtil;
using System.Security.Permissions;
using System.IO;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace DSWControl.DSWRichTextBox
{
    /// --------------------------------------------------
    /// <summary>
    /// リッチテキストボックスコントロール
    /// </summary>
    /// <create>D.Okumura 2019/06/13</create>
    /// <update></update>
    /// --------------------------------------------------
#if (!DEBUG)
    [System.Diagnostics.DebuggerStepThrough()]
#endif
    [ClassInterface(ClassInterfaceType.AutoDispatch), Designer(typeof(DSWControl.DSWRichTextBox.Design.DSWRichTextBoxDesigner)), ComVisible(true)]
    [ToolboxBitmap(typeof(DSWRichTextBox), "DSWRichTextBox.bmp")]
    [DefaultEvent("ValueChanged")]
    public partial class DSWRichTextBox : RichTextBox5
    {
        #region 列挙体
        /// --------------------------------------------------
        /// <summary>
        /// 選択した時のカーソル位置
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public enum SelectionTypeInner
        {
            /// --------------------------------------------------
            /// <summary>
            /// 元々選択されていた位置
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Original = 0,
            /// --------------------------------------------------
            /// <summary>
            /// 全選択
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            All = 1,
            /// --------------------------------------------------
            /// <summary>
            /// 最後尾
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Bottom = 2,
        }
        /// --------------------------------------------------
        /// <summary>
        /// 貼り付け可能種別
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public enum PastableTypeInner
        {
            /// --------------------------------------------------
            /// <summary>
            /// 全てのオブジェクト
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            [Description("書式情報を除くすべてのオブジェクト")]
            AllObjects,
            /// --------------------------------------------------
            /// <summary>
            /// 画像のみ
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            [Description("画像のみ、貼り付け時に異なるものがあっても無視して貼り付ける")]
            PicturesOnly,
            /// --------------------------------------------------
            /// <summary>
            /// 画像のみ
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            [Description("画像のみ、貼り付け時に異なるものがある場合は警告する")]
            PicturesOnlyWarnIfContains,
            /// --------------------------------------------------
            /// <summary>
            /// 画像のみ
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            [Description("画像のみ、貼り付け時に異なるものがある場合はエラーとする")]
            PicturesOnlyErrorIfContains,
        }
        #endregion

        #region フィールド

        private Color _focusBackColor;
        private bool _maxByteLengthMode;
        private string _inputRegulation;
        private bool _isInputRegulation;
        private int _maxLineCount;
        private int _oneLineMaxCount;
        private Dictionary<Int32, String> _prohibitionChar;
        private SelectionTypeInner _selectionPos;
        private bool _firstMouseDown = false;

        // 入力規制文字列を格納してる
        private Dictionary<Int32, String> _dict;
        // 背景色の退避先
        private Color _defaultColor;
        
        const Keys KEY_CUT_DEL = Keys.Control | Keys.Shift | Keys.Delete;
        const Keys KEY_CUT_X = Keys.Control | Keys.X;
        const Keys KEY_COPY_C = Keys.Control | Keys.C;
        const Keys KEY_COPY_INS = Keys.Control | Keys.Insert;
        const Keys KEY_PASTE_V = Keys.Control | Keys.V;
        const Keys KEY_PASTE_INS_SHIFT = Keys.Control | Keys.Insert | Keys.Shift;
        const Keys KEY_PASTE_INS = Keys.Shift | Keys.Insert;
        /// --------------------------------------------------
        /// <summary>
        /// 改行コード
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// --------------------------------------------------
        private readonly string[] BREAK_CODE = new string[] { "\r\n", "\r", "\n" };
        /// --------------------------------------------------
        /// <summary>
        /// 値が変更されたかどうか
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isValueChanged = false;
        /// --------------------------------------------------
        /// <summary>
        /// 背景色が変更されたかどうか
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isBackColorChanged = false;

        /// --------------------------------------------------
        /// <summary>
        /// 貼り付け可能なデフォルトの最大サイズ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int _defaultMaxPastableObjectSize = 0;

        /// --------------------------------------------------
        /// <summary>
        /// 貼り付ける際のJPEG画像圧縮クォリティ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private const long _defaultPasteJpegQuality = 100L;

        /// --------------------------------------------------
        /// <summary>
        /// クリップボード内の状態
        /// </summary>
        /// <create>D.Okumura 2019/06/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _hasObjectInClipboard = false;
        /// --------------------------------------------------
        /// <summary>
        /// クリップボード内の状態(アプリケーション内共通)
        /// </summary>
        /// <create>D.Okumura 2019/06/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private static bool _globalHasObjectInClipboard = false;

        #endregion //フィールド

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DSWRichTextBox()
        {
            InitializeComponent();

            this.FocusBackColor = base.BackColor;
            this._defaultColor = base.BackColor;

            this._dict = new Dictionary<int, string>();

            this.MaxByteLengthMode = false;
            this.InputRegulation = string.Empty;
            this.IsInputRegulation = false;
            this.MaxLineCount = 0;
            this.OneLineMaxLength = this.MaxLength;
            this.SelectionPos = SelectionTypeInner.Original;
            // デフォルト値の設定
            this.PastableType = PastableTypeInner.AllObjects;
            // 言語オプション: UIフォント固定
            this.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            this.richTextBoxPasteArea.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            // 貼り付け画像JPEG圧縮率
            this.PasteJpegQuality = _defaultPasteJpegQuality;
        }

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// フォーカス時のバックカラーの取得、設定
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム表示")]
        [Description("フォーカスが当たったときの背景色の設定を行えます。")]
        [DefaultValue(typeof(SystemColors), "Window")]
        public Color FocusBackColor
        {
            get { return _focusBackColor; }
            set { _focusBackColor = value; Invalidate(); }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 入力最大長がbyteか文字数か
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("True にすると入力最大長をbyteで検証します。False の場合は従来どおり文字数で検証します。")]
        [DefaultValue(false)]
        public bool MaxByteLengthMode
        {
            get { return _maxByteLengthMode; }
            set { _maxByteLengthMode = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 入力規制の取得、設定
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("入力規制の設定を行います。選択した種類が IsInputRegulation プロパティの対象となります。一つも選択しない場合はすべて使える状態になります。")]
        [Editor(typeof(InputRegulationEditor),
                typeof(System.Drawing.Design.UITypeEditor))]
        public string InputRegulation
        {
            get { return _inputRegulation; }
            set { _inputRegulation = value; _dict[0] = _inputRegulation; }
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 入力規制にセットしたものを使用可にするのか、不可にするのか
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        [Category("カスタム動作")]
        [Description("True にすると InputRegulation プロパティの対象が使用可能になります。False の場合は InputRegulation プロパティの対象以外が使用可能になります。")]
        [DefaultValue(false)]
        public bool IsInputRegulation
        {
            get { return _isInputRegulation; }
            set { _isInputRegulation = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 禁止文字の取得、設定
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("禁止文字の設定を行えます。")]
        [Editor(typeof(ProhibitionCharEditor),
                typeof(System.Drawing.Design.UITypeEditor))]
        public Dictionary<Int32, String> ProhibitionChar
        {
            get { return _prohibitionChar; }
            set { _prohibitionChar = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 貼り付け可能な画像の最大サイズ
        /// </summary>
        /// <remarks>
        /// 0の場合、チェックを行いません
        /// </remarks>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("貼り付け可能な最大サイズ")]
        [DefaultValue(_defaultMaxPastableObjectSize)]
        public virtual long MaxPastableObjectSize { get; set; }

        /// --------------------------------------------------
        /// <summary>
        /// 貼り付ける際のJPEG画像圧縮クォリティ
        /// </summary>
        /// <create>D.Okumura 2019/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("貼り付ける際のJPEG画像圧縮クォリティ")]
        [DefaultValue(_defaultPasteJpegQuality)]
        protected virtual long PasteJpegQuality { get; set; }

        /// --------------------------------------------------
        /// <summary>
        /// 何行入力できるか？
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("複数行に設定したテキストコントロールに入力できる最大行数を取得または設定します。")]
        [DefaultValue(0)]
        public int MaxLineCount
        {
            get { return _maxLineCount; }
            set
            {
                if (value < 0)
                {
                    throw new Exception();
                }
                _maxLineCount = value;
                /*
                if (value == 0)
                {
                    this.Multiline = false;
                }
                else
                {
                    this.Multiline = true;
                }
                 * */
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 貼り付け可能なオブジェクトの種類
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("貼り付け可能なオブジェクトの種類を取得または設定します。")]
        [DefaultValue(PastableTypeInner.AllObjects)]
        public PastableTypeInner PastableType { get; set; }

        /// --------------------------------------------------
        /// <summary>
        /// 一行に入力可能な文字数
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("一行に入力できる最大文字数を指定します。")]
        [DefaultValue(0)]
        public int OneLineMaxLength
        {
            get { return _oneLineMaxCount; }
            set
            {
                if (value < 0)
                {
                    DSWUtil.MsgBoxEx.ShowError("OneLineMaxLength に 0 より小さな値を設定することはできません。");
                    return;
                }
                if (this.MaxLength < value)
                {
                    DSWUtil.MsgBoxEx.ShowError("OneLineMaxLength に MaxLength より大きな値を設定することはできません。");
                    return;
                }
                _oneLineMaxCount = value;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 最大文字数
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public override int MaxLength
        {
            get
            {
                return base.MaxLength;
            }
            set
            {
                if (base.MaxLength <= this.OneLineMaxLength)
                {
                    base.MaxLength = value;
                    this.OneLineMaxLength = value;
                }
                else
                {
                    base.MaxLength = value;
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 選択した時のカーソル位置を取得・設定します。
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("選択した時のカーソル位置を取得・設定します。")]
        [DefaultValue(typeof(DSWControl.DSWRichTextBox.DSWRichTextBox.SelectionTypeInner), "Original")]
        public SelectionTypeInner SelectionPos
        {
            get { return _selectionPos; }
            set { _selectionPos = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// コントロールの背景色を取得または設定します。
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                // BackColorがユーザーの手によって変更されていない時
                // フォーカス無し -> SystemColors.Window
                // フォーカス有り -> FocusBackColor
                // ReadOnlyフォーカス無し -> SystemColors.Control
                // ReadOnlyフォーカス有り -> SystemColors.Control
                // Enabled -> SystemColors.Control
                // BackColorがユーザーの手によって変更されている時
                // フォーカス無し -> 変更後のBackColor
                // フォーカス有り -> FocusBackColor
                // ReadOnlyフォーカス無し -> 変更後のBackColor
                // ReadOnlyフォーカス有り -> 変更後のBackColor
                // Enabled -> 変更後のBackColor
                if (value.Equals(new Color()))
                {
                    this._isBackColorChanged = false;
                }
                else
                {
                    this._isBackColorChanged = true;
                }
                this._defaultColor = value;
                base.BackColor = value;

            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 選択範囲のテキスト
        /// </summary>
        /// <remarks>
        /// リッチテキストボックスでは改行コードはLFのみであるため、
        /// 適切にCRFLへ置き換える。
        /// </remarks>
        /// <create>D.Okumura 2019/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public override string Text
        {
            get
            {
                return base.Text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
            }
            set
            {
                base.Text = value;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 選択範囲のテキスト
        /// </summary>
        /// <remarks>
        /// リッチテキストボックスでは改行コードはLFのみであるため、
        /// 適切にCRFLへ置き換える。
        /// </remarks>
        /// <create>D.Okumura 2019/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public override string SelectedText
        {
            get
            {
                return base.SelectedText.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
            }
            set
            {
                base.SelectedText = value;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 選択範囲にオブジェクトがあるか
        /// </summary>
        /// <create>D.Okumura 2019/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool SelectionHasObject
        {
            get
            {
                return (this.SelectionType & RichTextBoxSelectionTypes.MultiObject) == RichTextBoxSelectionTypes.MultiObject
                    || (this.SelectionType & RichTextBoxSelectionTypes.Object) == RichTextBoxSelectionTypes.Object
                ;
            }
        }
        #endregion

        #region Events

        /// --------------------------------------------------
        /// <summary>
        /// 文字列変更イベント
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("ユーザーが文字列を変更し、コントロールからフォーカスが外れた後に発生します。")]
        public event EventHandler ValueChanged;


        /// --------------------------------------------------
        /// <summary>
        /// 貼り付け動作イベント
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("ユーザーが貼り付け操作を行った際に発生します")]
        public event EventHandler<DSWRichTextBoxPasteEventArgs> Pasted;
        #endregion

        #region OnEnter

        /// --------------------------------------------------
        /// <summary>
        /// Enter イベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (this.Enabled)
            {
                // Enabled = trueであればReadOnlyでも構わない
                if (this.SelectionPos == SelectionTypeInner.Original)
                {
                    // 処理無し
                }
                else if (this.SelectionPos == SelectionTypeInner.All)
                {
                    this.SelectAll();
                }
                else if (this.SelectionPos == SelectionTypeInner.Bottom)
                {
                    this.SelectionStart = this.Text.Length;
                    this.SelectionLength = 0;
                }
            }

            if (!this.ReadOnly)
            {
                base.BackColor = this.FocusBackColor;
            }
        }

        #endregion

        #region OnMouseDown

        /// --------------------------------------------------
        /// <summary>
        /// MouseDownイベントを発生させます
        /// </summary>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this._firstMouseDown)
            {
                if (this.SelectionPos == SelectionTypeInner.Original)
                {
                    // 処理無し
                }
                else if (this.SelectionPos == SelectionTypeInner.All)
                {
                    this.SelectAll();
                }
                else if (this.SelectionPos == SelectionTypeInner.Bottom)
                {
                    this.SelectionStart = this.Text.Length;
                    this.SelectionLength = 0;
                }
                this._firstMouseDown = false;
            }
            base.OnMouseDown(e);
        }

        #endregion

        #region OnMouseEnter

        /// --------------------------------------------------
        /// <summary>
        /// MouseEnterイベントを発生させます
        /// </summary>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnMouseEnter(EventArgs e)
        {
            this._firstMouseDown = !this.Focused;
            base.OnMouseEnter(e);
        }

        #endregion

        #region OnMouseLeave

        /// --------------------------------------------------
        /// <summary>
        /// MouseLeaveイベントを発生させます
        /// </summary>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnMouseLeave(EventArgs e)
        {
            this._firstMouseDown = false;
            base.OnMouseLeave(e);
        }

        #endregion

        #region OnLeave

        /// --------------------------------------------------
        /// <summary>
        /// Leave イベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this._firstMouseDown = false;

            if (this.ReadOnly || !this.Enabled)
            {
                return;
            }
            base.BackColor = this._defaultColor;

            if (this.ValueChanged != null && this._isValueChanged)
            {
                this._isValueChanged = false;
                this.ValueChanged(this, new EventArgs());
            }
        }

        #endregion

        #region OnEnabledChanged

        /// --------------------------------------------------
        /// <summary>
        /// EnabledChanged イベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            base.BackColor = this._defaultColor;
            if (this._isBackColorChanged)
            {
                if (this.ReadOnly || !this.Enabled || !this.Focused)
                {
                    base.BackColor = this._defaultColor;
                }
                else
                {
                    base.BackColor = this.FocusBackColor;
                }
            }
            else
            {
                if (this.ReadOnly || !this.Enabled)
                {
                    base.BackColor = SystemColors.Control;
                }
                else if (this.Focused)
                {
                    base.BackColor = this.FocusBackColor;
                }
                else
                {
                    base.BackColor = this._defaultColor;
                }
            }
        }

        #endregion

        #region OnReadOnlyChanged

        /// --------------------------------------------------
        /// <summary>
        /// ReadOnlyChanged イベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnReadOnlyChanged(EventArgs e)
        {
            base.OnReadOnlyChanged(e);

            base.BackColor = this._defaultColor;
            if (this._isBackColorChanged)
            {
                if (this.ReadOnly || !this.Enabled || !this.Focused)
                {
                    base.BackColor = this._defaultColor;
                }
                else
                {
                    base.BackColor = this.FocusBackColor;
                }
            }
            else
            {
                if (this.ReadOnly || !this.Enabled)
                {
                    base.BackColor = SystemColors.Control;
                }
                else if (this.Focused)
                {
                    base.BackColor = this.FocusBackColor;
                }
                else
                {
                    base.BackColor = this._defaultColor;
                }
            }
        }

        #endregion

        #region 入力バイト制限用
        /// --------------------------------------------------
        /// <summary>
        /// WndProcメソッド
        /// </summary>
        /// <param name="m"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [System.Diagnostics.DebuggerStepThrough()]
        protected override void WndProc(ref Message m)
        {
            const int WM_CHAR = 0x0102;
            const int WM_PASTE = 0x0302;
            const int WM_DELETE = 0x100;


            switch (m.Msg)
            {
                case WM_CHAR:
                    KeyPressEventArgs eKeyPress = new KeyPressEventArgs((char)(m.WParam.ToInt32()));
                    this.OnChar(eKeyPress);
                    if (eKeyPress.Handled)
                    {
                        return;
                    }
                    this._isValueChanged = true;
                    break;
                case WM_DELETE:
                    KeyEventArgs eKey = new KeyEventArgs((Keys)(m.WParam.ToInt32()));
                    this.OnChar(eKey);

                    if (eKey.Handled)
                    {
                        return;
                    }
                    break;
                case WM_PASTE:
                    //EN_CLIPFORMAT(0x0712) をハンドリングしたいが、ハンドリングできない
                    this.Paste();
                    return;
/*
                case 0x0101: //WM_KEYUP
                case 0x000F: //WM_PAINT
                case 0x0020: //WM_SETCURSOR
                case 0x0200: //WM_MOUSEMOVE
                case 0x0084: //WM_NCHITTEST		マウス カーソルが移動したことを示します
                case 0x0087: //WM_GETDLGCODE ダイアログ プロシージャがコントロール入力を処理できるようにします。
                case 0x204E: //OCM_NOTIFY
                    break;
                case 0x00B8: //EM_GETMODIFY 更新があったときにハンドリングされる
                    System.Diagnostics.Debug.WriteLine(string.Format("msg: EM_GETMODIFY, LParam: {1,0:X8} , WParam: {2,0:X8}", m.Msg, m.LParam.ToInt64(), m.WParam.ToInt64()));
                    break;
                case 0x2111: //OCM_COMMAND
                    System.Diagnostics.Debug.WriteLine(string.Format("msg: OCM_COMMAND, LParam: {1,0:X8} , WParam: {2,0:X8}", m.Msg, m.LParam.ToInt64(), m.WParam.ToInt64()));
                    break;
*/
                default:
                    //System.Diagnostics.Debug.WriteLine(string.Format("msg: {0,0:X4}, LParam: {1,0:X8}", m.Msg, m.LParam.ToInt64()));
                    break;
            }

            base.WndProc(ref m);
        }

        /// --------------------------------------------------
        /// <summary>
        /// OCM_COMMANDのハンドリング
        /// </summary>
        /// <param name="m">メッセージ</param>
        /// <param name="keyData">コマンドキー</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update>D.Okumura 2019/06/26 Closeが遅い問題を修正</update>
        /// --------------------------------------------------
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            if (keyData == KEY_PASTE_V
                || keyData == KEY_PASTE_INS
                || keyData == KEY_PASTE_INS_SHIFT)
            {
                this.Paste();
                m.Result = IntPtr.Zero;
                return true;
            }
            else if (keyData == KEY_CUT_X
                || keyData == KEY_CUT_DEL)
            {
                this.Cut();
                m.Result = IntPtr.Zero;
                return true;
            }
            else if (keyData == KEY_COPY_C
                || keyData == KEY_COPY_INS)
            {
                this.Copy();
                m.Result = IntPtr.Zero;
                return true;
            }
            //System.Diagnostics.Debug.WriteLine(string.Format("pck: OCM_COMMAND, LParam: {1,0:X8} , WParam: {2,0:X8}, key:{3}", m.Msg, m.LParam.ToInt64(), m.WParam.ToInt64(), keyData));
            return base.ProcessCmdKey(ref m, keyData);
        }

        #region OnPaste
        /// --------------------------------------------------
        /// <summary>
        /// ペーストイベント処理
        /// </summary>
        /// <param name="status">ステータス情報</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void OnPasteEvent(DSWRichTextBoxPasteEventStatus status)
        {
            OnPasteEvent(new DSWRichTextBoxPasteEventArgs(status));
        }

        /// --------------------------------------------------
        /// <summary>
        /// ペーストイベント処理
        /// </summary>
        /// <param name="arg">イベント情報</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void OnPasteEvent(DSWRichTextBoxPasteEventArgs arg)
        {

            System.Diagnostics.Debug.WriteLine(string.Format("event: {0}", arg.Status));
            if (this.Pasted == null)
                return;
            Pasted(this, arg);
        }


        /// --------------------------------------------------
        /// <summary>
        /// 貼り付け可能状態
        /// </summary>
        /// <returns>可否</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool CanPaste()
        {
            return CheckPaste() == DSWRichTextBoxPasteEventStatus.Success;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 貼り付け可能状態取得
        /// </summary>
        /// <returns>結果</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private DSWRichTextBoxPasteEventStatus CheckPaste()
        {
            if (this.ReadOnly || !this.Enabled)
            {
                return DSWRichTextBoxPasteEventStatus.None;
            }

            System.Diagnostics.Debug.WriteLine(string.Format("paste format: {0}", string.Join(",", Clipboard.GetDataObject().GetFormats())));
            if (Clipboard.ContainsText())
                return DSWRichTextBoxPasteEventStatus.Success;
            // Paste処理と判定が異なる: Paste処理ではクリップボード内のデータを使って貼り付け可能かチェックしているため、ここではおおよその判断をする
            if (Clipboard.ContainsImage())
                return DSWRichTextBoxPasteEventStatus.Success;
            if (Clipboard.ContainsData(DataFormats.Rtf))
                return DSWRichTextBoxPasteEventStatus.Success;
            if (this.PastableType == PastableTypeInner.AllObjects && Clipboard.ContainsFileDropList())
                return DSWRichTextBoxPasteEventStatus.Success;
            return DSWRichTextBoxPasteEventStatus.ErrorNoData;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ペースト時の処理
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public new void Paste()
        {
            try
            {
                PasteInner();
            }
            catch (OutOfMemoryException)
            {
                OnPasteEvent(DSWRichTextBoxPasteEventStatus.ErrorNoEnoughMemory);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Paste exception: {0}", ex));
                OnPasteEvent(DSWRichTextBoxPasteEventStatus.Error);
            }
        }



        /// --------------------------------------------------
        /// <summary>
        /// ペースト時の処理
        /// </summary>
        /// <create>D.Okumura 2019/06/22</create>
        /// <update></update>
        /// <remarks>
        /// IME有効な場合、OnCharイベントが発生せず、
        /// 有効なトリガがないため、テキスト入力が確定した時点で
        /// 入力文字以内の確認を行う。
        /// </remarks>
        /// --------------------------------------------------
        protected override void OnTextChanged(EventArgs e)
        {
            var input = this.Text;
            var allText = new CheckInputRegulation(_isInputRegulation, this.Multiline).StartCheck(input, _dict);
            allText = new CheckProphitionChar().StartCheck(allText, _prohibitionChar);
            if (!string.Equals(input, allText))
            {
                // 入力チェックエラーがある場合ロールバックする
                this.BeginInvoke(new Action(() => { if (this.CanUndo) this.Undo(); }));
                return;
            }
            if (this.MaxByteLengthMode)
            {
                System.Text.Encoding sjisEnc = Encoding.GetEncoding(UtilEncoding.SHIFT_JIS);
                int textByteCount = sjisEnc.GetByteCount(input);
                if (textByteCount > this.MaxLength)
                {
                    // 入力位置から文字を削って制限文字数以内にすることを検討したが、
                    // 元に戻す操作ができなくなるため、前の操作を完全に取り消す方針とする
                    /*
                    while ((textByteCount - sjisEnc.GetByteCount(this.SelectedText)) > this.MaxLength)
                    {
                        if (this.SelectionStart > 0)
                            this.SelectionStart--;
                        this.SelectionLength++;
                    }
                    this.SelectedText = string.Empty;
                    */
                    // このイベント内でUndoをコールすることはできないため、
                    // ディスパッチ後に実施する。
                    // この方法でやり直しが実行されたとしても、このロジックに入り、
                    // やり直しに対する元に戻すが発動し、強制的に元に戻される想定
                    this.BeginInvoke(new Action(() => { if (this.CanUndo) this.Undo(); }));
                    return;
                }
            }
            else
            {
                // 入力文字数での制限については実装不要の認識
            }
            base.OnTextChanged(e);
        }

        
        /// --------------------------------------------------
        /// <summary>
        /// ペースト時の処理
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update>D.Okumura 2019/06/27 10M準拠対応:画像をJPEG形式へ変換</update>
        /// <update>D.Okumura 2019/07/01 IBM Notesからの貼り付け不具合対応</update>
        /// --------------------------------------------------
        private void PasteInner()
        {
            var result = CheckPaste();
            if (result != DSWRichTextBoxPasteEventStatus.Success)
            {
                if (result != DSWRichTextBoxPasteEventStatus.None)
                    OnPasteEvent(result);
                return;
            }
            DSWRichTextBoxData data;
            // クリップボードの画像・文字列取得→内部リッチテキストボックス
            string inputText;
            if (Clipboard.ContainsImage() || Clipboard.ContainsData(DataFormats.Rtf) || Clipboard.ContainsFileDropList())
            {
                // Bitmapが含まれるとクリップボード上に記載があっても、画像データが取得できないことがあるため、
                // リッチテキスト→画像→その他　の順序で貼り付けを試みる

                if (Clipboard.ContainsData(DataFormats.Rtf))
                {
                    // リッチテキスト形式の場合
                    if (!PasteToRechTextFromClipboard(this.richTextBoxPasteArea))
                    {
                        // RTFデータの加工に失敗した場合エラーに倒す
                        OnPasteEvent(DSWRichTextBoxPasteEventStatus.ErrorNoData);
                        return;
                    }
                }
                else if (Clipboard.ContainsImage() && !Clipboard.ContainsText())
                {
                    // 画像のみの場合はJpeg形式で貼り付ける
                    var jpg = DataFormats.GetFormat("JFIF");
                    var bitmap = DataFormats.GetFormat(DataFormats.Bitmap);

                    if (this.richTextBoxPasteArea.CanPaste(jpg))
                    {
                        this.richTextBoxPasteArea.Paste(jpg);
                    }
                    else if (this.richTextBoxPasteArea.CanPaste(bitmap))
                    {
                        // ビットマップ形式の場合、JPEG形式へ変換する
                        if (ConvertClipboardBitmapToJpeg(PasteJpegQuality))
                        {
                            this.richTextBoxPasteArea.Paste(jpg);
                        }
                        else
                        {
                            // bitmap形式で貼り付けできない場合はエラーに倒す
                            OnPasteEvent(DSWRichTextBoxPasteEventStatus.ErrorNoData);
                            return;
                        }
                    }
                    else
                    {
                        // bitmap形式で貼り付けできない場合はエラーに倒す
                        OnPasteEvent(DSWRichTextBoxPasteEventStatus.ErrorNoData);
                        return;
                    }
                }
                else
                {
                    // 自動判別で貼り付けを行う
                    this.richTextBoxPasteArea.Paste();
                }


                // リッチテキスト内の画像データの圧縮処理(全般処理)
                {
                    // 画像データの変換を行う
                    bool isClipboardUsed = false;
                    // 改行位置などの違いがあるため、全体をなめる
                    for (var pos = 0; pos < this.richTextBoxPasteArea.TextLength; pos++)
                    {
                        this.richTextBoxPasteArea.Select(pos, 1);
                        if (this.richTextBoxPasteArea.SelectionType == RichTextBoxSelectionTypes.Object)
                        {
                            // RTF文字列は巨大なので、局所化する
                            {
                                string rtf = this.richTextBoxPasteArea.SelectedRtf;
                                // 画像ではないまたはJPEG画像である場合はスキップする
                                if (!rtf.Contains("pict") || rtf.Contains("jpegblip"))
                                    continue;
                            }
                            // コピー操作を行う
                            this.richTextBoxPasteArea.Copy();
                            isClipboardUsed = true;
                            System.Diagnostics.Debug.WriteLine(string.Format("{0}: 変換開始", pos));
                            // コピー操作を行うと、画像情報が乗るので、その情報用いて貼り付ける(Windows10)
                            var jpg = DataFormats.GetFormat("JFIF");
                            if (this.richTextBoxPasteArea.CanPaste(jpg))
                            {
                                this.richTextBoxPasteArea.Paste(jpg);
                                System.Diagnostics.Debug.WriteLine(string.Format("{0}: JPEG貼付成功", pos));
                            }
                            else
                            {
                                // Emf形式で画像が乗るはずなので、その画像を変換する
                                if (!ConvertClipboardEmfToJpeg(PasteJpegQuality))
                                {
                                    System.Diagnostics.Debug.WriteLine(string.Format("{0}: EMF→JPEG変換失敗", pos));
                                }
                                // JPEG形式でクリップボードに乗る想定なので、貼り付け操作を行う
                                if (this.richTextBoxPasteArea.CanPaste(jpg))
                                {
                                    this.richTextBoxPasteArea.Paste(jpg);
                                    System.Diagnostics.Debug.WriteLine(string.Format("{0}: EMF→JPEG貼付成功", pos));
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(string.Format("{0}: EMF→JPEG貼付失敗", pos));
                                }
                            }
                        }
                    }
                    // 変換にクリップボードが使用された場合、クリアする
                    if (isClipboardUsed)
                        Clipboard.Clear();
                }

                this.richTextBoxPasteArea.Select(0, 0);
                data = new DSWRichTextBoxData();
                // 貼り付けたリッチテキストから画像とテキストを分離
                // このとき、抽出するデータを限定しない
                GetImageData(this.richTextBoxPasteArea, false, data.Images);
                data.Text = this.richTextBoxPasteArea.Text;
                inputText = data.Text;
                this.richTextBoxPasteArea.Clear();
                this.richTextBoxPasteArea.ClearUndo();
                // 貼り付け可能サイズのチェックを行う (1枚のみ貼り付けるときのみ)
                if (this.MaxPastableObjectSize != 0 && data.Images.Count == 1 && inputText.Length == 1)
                {
                    foreach (var item in data.Images)
                    {
                        if (item.CompressedData.LongLength > this.MaxPastableObjectSize)
                        {
                            OnPasteEvent(DSWRichTextBoxPasteEventStatus.ErrorSizeOver);
                            return;
                        }
                    }
                }
                // 画像以外を含む場合はエラーとする設定の時、貼り付け操作を中断
                if (this.PastableType == PastableTypeInner.PicturesOnlyErrorIfContains && data.Images.Exists(w => w.IsObject))
                {
                    OnPasteEvent(DSWRichTextBoxPasteEventStatus.ErrorContainsNonImageData);
                    return;
                }
                // 入力文字が存在しない場合、エラーに倒す
                if (string.IsNullOrEmpty(inputText))
                {
                    OnPasteEvent(DSWRichTextBoxPasteEventStatus.ErrorNoData);
                    return;
                }
            }
            // テキストのみ
            else
            {
                data = null;
                inputText = Clipboard.GetText();
            }

            inputText = new CheckInputRegulation(_isInputRegulation, this.Multiline).StartCheck(inputText, _dict);
            inputText = new CheckProphitionChar().StartCheck(inputText, _prohibitionChar);
            // \r -> \r\n
            // \n -> \r\n
            // \r\n -> \r\r\n\r\n -> \r\n
            inputText = inputText.Replace("\r", "\r\n").Replace("\n", "\r\n").Replace("\r\r\n\r\n", "\r\n");
            // 複数行不可の場合、この段階で改行コードは消す
            if (!this.Multiline)
            {
                inputText = inputText.Replace("\r\n", string.Empty);
            }
            string selectedText = this.SelectedText;
            if (data != null)
            {
                // 画像の貼り付けであるにもかかわらず、貼り付け文字列が空白となる場合は入力チェックに問題があるため、貼り付け操作をエラーとする
                if (!inputText.Contains(" ") && data.Images.Count > 0)
                {
                    OnPasteEvent(DSWRichTextBoxPasteEventStatus.ErrorInvalidInputReguration);
                    return;
                }
            }

            // 行数制御処理
            if (0 < this.MaxLineCount)
            {
                string[] textParts = this.Text.Split(BREAK_CODE, StringSplitOptions.None);
                if (this.MaxLineCount < textParts.Length)
                {
                    // 文字列が最大行数を超えていたらペースト不可
                    OnPasteEvent(DSWRichTextBoxPasteEventStatus.ErrorLengthOver);
                    return;
                }
                string[] inputTextParts = inputText.Split(BREAK_CODE, StringSplitOptions.None);
                inputText = string.Empty;
                int partsCount = 0;
                int loopStart = 0 < textParts.Length ? textParts.Length - 1 : textParts.Length;
                for (int i = loopStart; i < this.MaxLineCount; i++)
                {
                    if (partsCount == inputTextParts.Length)
                    {
                        break;
                    }
                    // 既存コード不具合：不用意な改行コードが付与される不具合を修正
                    //if (i != this.MaxLineCount - 1)
                    if (i != this.MaxLineCount - 1 && (partsCount != inputTextParts.Length-1))
                    {
                        inputText += inputTextParts[partsCount] + Environment.NewLine;
                    }
                    else
                    {
                        inputText += inputTextParts[partsCount];
                    }
                    partsCount++;
                }
                selectedText = inputText;
            }

            if (this.MaxByteLengthMode)
            {
                Encoding sjisEnc = Encoding.GetEncoding(UtilEncoding.SHIFT_JIS);
                // 貼り付け前のバイト数
                int textByteCount = sjisEnc.GetByteCount(this.Text);
                // 貼り付けるバイト数
                int inputByteCount = sjisEnc.GetByteCount(inputText);
                // 選択されているバイト数
                int selectedTextByteCount = sjisEnc.GetByteCount(this.SelectedText);
                // 残りバイト数（後何バイト入るのか）
                int remainByteCount = this.MaxLength - (textByteCount - selectedTextByteCount);

                if (remainByteCount <= 0)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("no remain: {0} selected: {1} input: {2}", remainByteCount, selectedTextByteCount, inputByteCount));
                    // 文字列が最大数を超えていたらペースト不可
                    OnPasteEvent(DSWRichTextBoxPasteEventStatus.ErrorLengthOver);
                    return;
                }

                string beginText = UtilString.Substring(this.Text, 0, this.SelectionStart);
                string selectText = inputText;
                string endText = UtilString.Substring(this.Text, this.SelectionStart + this.SelectionLength);

                if (inputByteCount <= remainByteCount)
                {
                    selectText = inputText;
                }
                else
                {
                    inputText = MidByte(inputText, 0, remainByteCount);
                    selectText = inputText;
                }

                // 一行しか入力できないので後の処理は必要ない
                if (!this.Multiline)
                {
                    if (this.SelectedText != selectedText)
                    {
                        this._isValueChanged = true;
                    }
                    OnPasteInner(data, selectText);
                    return;
                }

                // 改行文字で入力されている文字列を分解
                string[] textParts = (beginText + selectText + endText).Split(BREAK_CODE, StringSplitOptions.None);

                string adjustText = string.Empty;
                for (int i = 0; i < textParts.Length; i++)
                {
                    if (this.OneLineMaxLength < sjisEnc.GetByteCount(textParts[i]))
                    {
                        adjustText += UtilString.SubstringForByte(textParts[i], 0, this.OneLineMaxLength);
                    }
                    else
                    {
                        adjustText += textParts[i];
                    }

                    // 改行コードを追加
                    if (i != textParts.Length - 1)
                    {
                        adjustText += Environment.NewLine;
                    }
                }
                adjustText = UtilString.Substring(adjustText, UtilString.Length(beginText));
                selectedText = UtilString.Substring(adjustText, 0, UtilString.Length(adjustText) - UtilString.Length(endText));
            }
            else
            {
                string beginText = UtilString.Substring(this.Text, 0, this.SelectionStart);
                string selectText = inputText;
                string endText = UtilString.Substring(this.Text, this.SelectionStart + this.SelectionLength);

                if (this.MaxLength < UtilString.Length(beginText) + UtilString.Length(selectText) + UtilString.Length(endText))
                {
                    selectText = UtilString.Substring(selectText, 0, this.MaxLength - UtilString.Length(beginText) - UtilString.Length(endText));
                }

                // 一行しか入力できないので後の処理は必要ない
                if (!this.Multiline)
                {
                    if (this.SelectedText != selectedText)
                    {
                        this._isValueChanged = true;
                    }
                    OnPasteInner(data, selectText);
                    return;
                }

                // 改行文字で入力されている文字列を分解
                string[] textParts = (beginText + selectText + endText).Split(BREAK_CODE, StringSplitOptions.None);

                string adjustText = string.Empty;
                for (int i = 0; i < textParts.Length; i++)
                {
                    if (this.OneLineMaxLength < UtilString.Length(textParts[i]))
                    {
                        adjustText += UtilString.Substring(textParts[i], 0, this.OneLineMaxLength);
                    }
                    else
                    {
                        adjustText += textParts[i];
                    }

                    // 改行コードを追加
                    if (i != textParts.Length - 1)
                    {
                        adjustText += Environment.NewLine;
                    }
                }
                adjustText = UtilString.Substring(adjustText, UtilString.Length(beginText));
                selectedText = UtilString.Substring(adjustText, 0, UtilString.Length(adjustText) - UtilString.Length(endText));
            }
            if (this.SelectedText != selectedText)
            {
                this._isValueChanged = true;
            }
            OnPasteInner(data, selectedText);
        }

        #region 画像変換処理

        /// --------------------------------------------------
        /// <summary>
        /// リッチテキストデータを貼り付け(ワークアラウンド対応)
        /// </summary>
        /// <param name="rich">リッチテキスト</param>
        /// <remarks>
        /// Windows 10のリッチテキストはfonttbl要素内のユニコード記述(\u)に
        /// 対応していないようであるため、fonttbl要素内から該当のフォント名を取り除き、
        /// 「{\fNN\fmodern \fcharset128 ;}」とする。
        /// クリップボードの中身は変更しない。
        /// また、デバッグ時以外は例外が発生してもフォールバックさせる。
        /// </remarks>
        /// <create>D.Okumura 2019/07/01 Notesからの貼り付けワークアラウンド</create>
        /// <update></update>
        /// --------------------------------------------------
        private static bool PasteToRechTextFromClipboard(RichTextBox rich)
        {
            const bool debugThis = false; //このメソッドをデバッグする場合のフラグ
            if (!Clipboard.ContainsData(DataFormats.Rtf))
                return false;
            string inputFile;
            {
                var data = Clipboard.GetData(DataFormats.Rtf) as string;
                if (data == null)
                    return false;
                // カッコで始まらないないとき、そのまま貼り付けて抜ける
                if (!data.StartsWith("{"))
                {
                    var rtf = DataFormats.GetFormat(DataFormats.Rtf);
                    rich.Paste(rtf);
                    return true;
                }
                inputFile = Path.GetTempFileName();
                File.WriteAllText(inputFile, data);
            }
            string outputFile = Path.GetTempFileName();
            try
            {
                // RTFを読み出してファイルへ保存する
                using (var input = new StreamReader(inputFile))
                using (var output = new StreamWriter(outputFile))
                {
                    int stage = 0;
                    int fonttblInner = 0;
                    var fonttblSb = new StringBuilder();
                    // 1行ずつ読みだして、必要な部分のみを抜き取る
                    while (!input.EndOfStream)
                    {
                        var line = input.ReadLine();
                        switch (stage)
                        {
                            //{\fonttbl{\f0....を探す
                            case 0: //初期状態
                                if (line.Contains(@"{\fonttbl"))
                                {
                                    //fonttblが見つかったら解析開始
                                    stage++;
                                    var idx = line.IndexOf(@"{\fonttbl");
                                    if (idx > 0)
                                    {
                                        output.Write(line.Substring(0, idx));
                                        fonttblSb = new StringBuilder();
                                        line = line.Substring(idx);
                                        fonttblInner = 0;
                                        Debug.WriteLineIf(debugThis, "start");
                                    }
                                    // fonttbl抽出、加工処理を行う
                                    goto case 1;
                                }
                                else
                                {
                                    output.WriteLine(line);
                                }
                                break;
                            case 1: //fonttbl抽出
                                var pos = 0;
                                var firstpos = pos;
                                var lastPos = pos;
                                // 入れ子の考慮を行いながら、データを取得する
                                do
                                {
                                    var checkFirst = line.IndexOf("{", firstpos);
                                    var checkLast = line.IndexOf("}", lastPos);
                                    if (checkFirst != -1 && (checkFirst < checkLast || checkLast == -1))
                                    {
                                        Debug.WriteLineIf(debugThis, string.Format("first {2} {0},{1}", checkFirst, checkLast, fonttblInner));
                                        fonttblInner++;
                                        firstpos = checkFirst + 1;
                                        lastPos = checkFirst + 1;
                                    }
                                    else if (checkLast == -1)
                                    {
                                        //Debug.WriteLine("go next");
                                        break;
                                    }
                                    else
                                    {
                                        Debug.WriteLineIf(debugThis, string.Format("last {2} {0},{1}", checkFirst, checkLast, fonttblInner));
                                        fonttblInner--;
                                        lastPos = checkLast + 1;
                                    }
                                } while (fonttblInner > 0);

                                if (fonttblInner == 0)
                                {
                                    // 解析完了
                                    if (line.Length >= lastPos)
                                    {
                                        fonttblSb.Append(line);
                                        line = "";
                                    }
                                    else
                                    {
                                        // lastPosが0未満は本処理上ありえない
                                        fonttblSb.Append(line.Substring(0, lastPos + 1));
                                        line = line.Substring(lastPos + 1);
                                    }
                                    // 処理
                                    // {\f5\fmodern .... を見つけ次第置換、入れ子は存在しない認識
                                    var regex = new Regex(@"(\{\\f\d+)([^\{\}]*)\}", RegexOptions.Multiline);
                                    var matches = regex.Matches(fonttblSb.ToString());
                                    foreach (Match match in matches)
                                    {
                                        if (match.Value.Contains(@"\u"))
                                        {
                                            // 見つけた部分を置換
                                            var replacement = match.Groups[1].Value + @"\fmodern \fcharset128 ;}";
                                            Debug.WriteLineIf(debugThis, string.Format("replace {0}->{1}", match.Value, replacement));
                                            fonttblSb = fonttblSb.Replace(match.Value, replacement);
                                        }
                                    }
                                    // 出力
                                    output.Write(fonttblSb);
                                    fonttblSb = new StringBuilder();

                                    // 次へ: 初期状態
                                    stage = 0;
                                    goto case 0;
                                }
                                else
                                {
                                    // 次の行を検索
                                    fonttblSb.Append(line);
                                }

                                break;
                        } //switch

                    } //while
                    // 余ったデータを吐き出し
                    output.Write(fonttblSb);

                } // using

                rich.LoadFile(outputFile, RichTextBoxStreamType.RichText);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Paste exception: {0}", ex));
                if (debugThis && (ex as ArgumentException) != null)
                {
                    Debug.WriteLineIf(debugThis, File.ReadAllText(outputFile));
                }
                // 救済措置:デバッグ以外の時はそのまま貼り付ける
                var rtf = DataFormats.GetFormat(DataFormats.Rtf);
                if (!debugThis && rich.CanPaste(rtf))
                {
                    rich.Paste(rtf);
                    return true;
                }
                return false;
            }
            finally
            {
                try
                {
                    if (File.Exists(inputFile))
                        File.Delete(inputFile);
                    if (File.Exists(outputFile))
                        File.Delete(outputFile);
                }
                catch { }
            }
            return true;
        }
        /// --------------------------------------------------
        /// <summary>
        /// クリップボード内のデータをJPEGへ変換
        /// </summary>
        /// <param name="quality">JPEG画質</param>
        /// <create>D.Okumura 2019/06/27</create>
        /// <update>K.Tsutsumi 2019/07/01 Notesからの貼付けでDIB対応</update>
        /// --------------------------------------------------
        private static bool ConvertClipboardBitmapToJpeg(long quality)
        {
            IDataObject tmp = Clipboard.GetDataObject();
            ImageCodecInfo jpegEncoder = null;

            foreach (ImageCodecInfo ici in ImageCodecInfo.GetImageEncoders())
            {
                if (ici.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpegEncoder = ici;
                    break;
                }
            }
            if (tmp == null || jpegEncoder == null)
            {
                return false;
            }

            // Bitmap形式の場合
            if (tmp.GetDataPresent(DataFormats.Bitmap))
            {
                Bitmap img = tmp.GetData(DataFormats.Bitmap) as Bitmap;
                if (img == null)
                {
                    // 取れなかった場合(DIB対応)
                    MemoryStream ms = (MemoryStream)tmp.GetData(DataFormats.Dib);
                    if (ms != null)
                    {
                        try
                        {
                            // DIBにヘッダを付けてBMP化する
                            img = CreateBitmapFromDIB(ms);
                        }
                        finally
                        {
                            ms.Dispose();
                        }
                    }                        
                }

                // 取得できなかった場合、処理を中断する
                if (img == null) return false;

                // 取得できた場合
                using (var stream = new MemoryStream())
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                    img.Save(stream, jpegEncoder, encParams);
                    var dataobject = new DataObject();
                    dataobject.SetData("JFIF", false, stream);
                    dataobject.SetImage(img);
                    Clipboard.SetDataObject(dataobject, true);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// DIB(DeviceIndependentBitmap)形式からBitmapオブジェクトを作成
        /// </summary>
        /// <param name="dib">DIBメモリストリーム</param>
        /// <returns>Bitmapオブジェクト</returns>
        /// <create>K.Tsutsumi 2019/07/01 Notesからの貼付けでDIB対応</create>
        /// <update></update>
        /// <remarks>
        /// MemoryStream等に対してメモリを解放(using構文を使用)していないのは、
        /// Bitmapオブジェクトの使用が終わるまでは解放できないため。
        /// </remarks>
        /// --------------------------------------------------
        private static Bitmap CreateBitmapFromDIB(MemoryStream dib)
        {
            const Int32 BITMAPFILEHEADER_SIZE = 14;

            if (dib == null) return null;

            byte[] bin = dib.ToArray();
            Int32 headerSize = BitConverter.ToInt32(bin, 0);
            Int32 pixelSize = bin.Length - headerSize;
            Int32 fileSize = BITMAPFILEHEADER_SIZE + bin.Length;
            Int16 reserved1 = 0;
            Int16 reserved2 = 0;

            MemoryStream bmpStm = new MemoryStream(fileSize);
            BinaryWriter writer = new BinaryWriter(bmpStm);

            //--- Bitmap File Header ---
            writer.Write(Encoding.ASCII.GetBytes("BM")); //Type
            writer.Write(fileSize); //Size(4byte)
            writer.Write(reserved1); //reserved1(2byte)
            writer.Write(reserved2); //reserved2(2byte)
            writer.Write(BITMAPFILEHEADER_SIZE + headerSize); //OffBits(4byte)

            //--- DIB ---
            writer.Write(bin);
            writer.Flush();
            bmpStm.Seek(0, SeekOrigin.Begin);
            return new Bitmap(bmpStm, false);
        }
        

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetClipboardData(uint format);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool IsClipboardFormatAvailable(uint format);

        /// --------------------------------------------------
        /// <summary>
        /// クリップボード内のデータをJPEGへ変換
        /// </summary>
        /// <param name="quality">JPEG画質</param>
        /// <create>D.Okumura 2019/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        static bool ConvertClipboardEmfToJpeg(long quality)
        {
            const uint CF_ENHMETAFILE = 14;
            ImageCodecInfo jpegEncoder = null;

            foreach (ImageCodecInfo ici in ImageCodecInfo.GetImageEncoders())
            {
                if (ici.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpegEncoder = ici;
                    break;
                }
            }
            if (jpegEncoder == null)
                return false;

            Metafile emf = null;
            if (OpenClipboard(IntPtr.Zero))
            {
                try
                {
                    if (IsClipboardFormatAvailable(CF_ENHMETAFILE))
                    {
                        IntPtr ptr = GetClipboardData(CF_ENHMETAFILE);
                        if (!ptr.Equals(IntPtr.Zero))
                            emf = new Metafile(ptr, true);
                    }
                }
                finally
                {
                    // 確実にCloseする
                    CloseClipboard();
                }
            }
            if (emf == null)
                return false;

            // JPEG画像へ変換し、クリップボードへ
            using (var stream = new MemoryStream())
            using (EncoderParameters encParams = new EncoderParameters(1))
            {
                encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                emf.Save(stream, jpegEncoder, encParams);
                var dataobject = new DataObject();
                dataobject.SetData("JFIF", false, stream);
                //dataobject.SetImage(emf);
                Clipboard.SetDataObject(dataobject, true);
            }
            emf.Dispose();
            return true;

        }
        #endregion
        /// --------------------------------------------------
        /// <summary>
        /// ペースト処理
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="inputText">テキストボックス入力値</param>
        /// <remarks>
        /// Windows8.1において文字の並びによってフォントが変わってしまう問題が
        /// 発生するため、貼り付け後にフォントをリセットする対応を行う。
        /// テキストのみの場合は発生していないので、フォントリセットは行わない。
        /// （フォントを変更すると履歴が積みあがってしまうため、無用な変更は行わない）
        /// </remarks>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update>D.Okumura 2019/07/02 フォントリセット対応</update>
        /// --------------------------------------------------
        private void OnPasteInner(DSWRichTextBoxData data, string inputText)
        {
            if (data == null || data.Images.Count < 1)
            {
                this.SelectedText = inputText;
                OnPasteEvent(DSWRichTextBoxPasteEventStatus.Success);
                return;
            }
            // 履歴をきれいにするため、一度別のリッチテキストで整形してから貼り付けを行う
            this.richTextBoxPasteArea.Clear();
            {
                var newData = data.ShrinkText(inputText);
                this.richTextBoxPasteArea.Text = newData.Text;
                this.richTextBoxPasteArea.Select(0, 0);
                SetImageData(this.richTextBoxPasteArea, PastableType != PastableTypeInner.AllObjects, newData.Images);
                // ワークアラウンド: フォントリセット
                this.richTextBoxPasteArea.SelectAll();
                this.richTextBoxPasteArea.SelectionFont = this.Font.Clone() as Font;
            }
            this.SelectedRtf = TrimRtfPara(this.richTextBoxPasteArea.Rtf);
            this.richTextBoxPasteArea.Clear();
            this.richTextBoxPasteArea.ClearUndo();
            if (this.PastableType == PastableTypeInner.PicturesOnlyWarnIfContains && data.Images.Exists(w => w.IsObject))
            {
                // 貼り付けデータに画像以外のデータが含まれていた
                OnPasteEvent(DSWRichTextBoxPasteEventStatus.WarnContainsNonImageData);
            }
            else
            {
                OnPasteEvent(DSWRichTextBoxPasteEventStatus.Success);
            }
        }

        #endregion

        #region コピー・切り取り操作

        /// --------------------------------------------------
        /// <summary>
        /// 切り取り処理
        /// </summary>
        /// <create>D.Okumura 2019/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public new void Cut()
        {
            if (this.ReadOnly)
                return;
            var hasObject = this.SelectionHasObject;
            this._hasObjectInClipboard = hasObject;
            _globalHasObjectInClipboard = hasObject;
            base.Cut();
        }
        /// --------------------------------------------------
        /// <summary>
        /// コピー
        /// </summary>
        /// <create>D.Okumura 2019/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public new void Copy()
        {
            var hasObject = this.SelectionHasObject;
            this._hasObjectInClipboard = hasObject;
            _globalHasObjectInClipboard = hasObject;
            base.Copy();
        
        }
        #endregion


        #region OnChar

        /// --------------------------------------------------
        /// <summary>
        /// キー入力時の処理(Deleteのみ)
        /// </summary>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void OnChar(KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
            {
                return;
            }

            // 入力されている文字列の最初から選択されている部分まで
            string beginText = UtilString.Substring(this.Text, 0, this.SelectionStart);
            if (beginText == this.Text)
            {
                e.Handled = true;
                return;
            }
            // 残りの文字列
            string endText;
            if (this.SelectedText != string.Empty)
            {
                endText = UtilString.Substring(this.Text, this.SelectionStart + this.SelectionLength);
            }
            else
            {
                endText = UtilString.Substring(this.Text, this.SelectionStart + this.SelectionLength + 1);
                if (endText.StartsWith("\n"))
                {
                    endText = UtilString.Remove(endText, 0, 1);
                }
            }

            // 改行文字で入力されている文字列を分解
            string[] textParts = (beginText + endText).Split(BREAK_CODE, StringSplitOptions.None);

            if (this.MaxByteLengthMode)
            {
                System.Text.Encoding sjisEnc = Encoding.GetEncoding(UtilEncoding.SHIFT_JIS);
                for (int i = 0; i < textParts.Length; i++)
                {
                    // バイト数でチェック
                    if (this.OneLineMaxLength < sjisEnc.GetByteCount(textParts[i]))
                    {
                        e.Handled = true;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < textParts.Length; i++)
                {
                    // 文字数でチェック
                    if (this.OneLineMaxLength < UtilString.Length(textParts[i]))
                    {
                        e.Handled = true;
                        break;
                    }
                }
            }

            if (!e.Handled)
            {
                this._isValueChanged = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// キー入力時の処理
        /// </summary>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void OnChar(KeyPressEventArgs e)
        {
            // Enter 入力時の処理
            if (0 < this.MaxLineCount)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    string[] textParts = this.Text.Split(BREAK_CODE, StringSplitOptions.None);
                    if (this.MaxLineCount == textParts.Length)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }

            if (e.KeyChar == (char)Keys.Back)
            {
                // 入力されている文字列の最初から選択されている部分まで
                string beginText = UtilString.Substring(this.Text, 0, this.SelectionStart);
                if (this.SelectionStart != 0 && this.SelectedText == string.Empty)
                {
                    beginText = UtilString.Remove(beginText, UtilString.Length(beginText) - 1);
                    if (beginText.EndsWith("\r"))
                    {
                        beginText = UtilString.Remove(beginText, UtilString.Length(beginText) - 1);
                    }
                }

                if (this.SelectionStart == 0 && string.IsNullOrEmpty(this.SelectedText))
                {
                    e.Handled = true;
                }
                // 残りの文字列
                string endText = UtilString.Substring(this.Text, this.SelectionStart + this.SelectionLength);

                // 改行文字で入力されている文字列を分解
                string[] textParts = (beginText + endText).Split(BREAK_CODE, StringSplitOptions.None);

                string adjustText = string.Empty;
                if (this.MaxByteLengthMode)
                {
                    System.Text.Encoding sjisEnc = Encoding.GetEncoding(UtilEncoding.SHIFT_JIS);
                    for (int i = 0; i < textParts.Length; i++)
                    {
                        // バイト数でチェック
                        if (this.OneLineMaxLength < sjisEnc.GetByteCount(textParts[i]))
                        {
                            e.Handled = true;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < textParts.Length; i++)
                    {
                        // 文字数でチェック
                        if (this.OneLineMaxLength < UtilString.Length(textParts[i]))
                        {
                            e.Handled = true;
                            break;
                        }
                    }
                }
            }

            if (char.IsControl(e.KeyChar)
                && e.KeyChar != (char)Keys.Enter)
            {
                return;
            }

            if (MaxByteLengthMode)
            {
                System.Text.Encoding sjisEnc = Encoding.GetEncoding(UtilEncoding.SHIFT_JIS);

                // 入力前のバイト数
                int textByteCount = sjisEnc.GetByteCount(this.Text);
                // 入力するバイト数
                int inputByteCount = sjisEnc.GetByteCount(e.KeyChar.ToString());
                if (e.KeyChar == (char)Keys.Enter)
                {
                    // Enterは2byteですよ
                    inputByteCount = 2;
                }
                // 選択されているバイト数
                int selectedTextByteCount = sjisEnc.GetByteCount(this.SelectedText);

                if (this.MaxLength < (textByteCount + inputByteCount - selectedTextByteCount))
                {
                    e.Handled = true;
                }

                if (!this.Multiline)
                {
                    string plusText = UtilString.Substring(this.Text, 0, this.SelectionStart) + e.KeyChar.ToString() + UtilString.Substring(this.Text, this.SelectionStart + this.SelectionLength);
                    if (string.IsNullOrEmpty((new CheckInputRegulation(_isInputRegulation, this.Multiline).StartCheck(e.KeyChar.ToString(), _dict))))
                    {
                        e.Handled = true;
                    }
                    else if (new CheckProphitionChar().StartCheck(plusText, _prohibitionChar) != plusText)
                    {
                        //Console.Beep(400, 100);
                        int start = this.SelectionStart;
                        this.Text = new CheckProphitionChar().StartCheck(plusText, _prohibitionChar);
                        this.Select(start, 0);
                        e.Handled = true;
                    }
                    return;
                }

                // 入力されている文字列の最初から選択されている部分まで
                string beginText = UtilString.Substring(this.Text, 0, this.SelectionStart);
                // 選択されている文字列
                string selectText = e.KeyChar.ToString();
                // 残りの文字列
                string endText = UtilString.Substring(this.Text, this.SelectionStart + this.SelectionLength);

                // 改行文字で入力されている文字列を分解
                string[] textParts = (beginText + selectText + endText).Split(BREAK_CODE, StringSplitOptions.None);

                string adjustText = string.Empty;
                for (int i = 0; i < textParts.Length; i++)
                {
                    // バイト数でチェック
                    if (this.OneLineMaxLength < sjisEnc.GetByteCount(textParts[i]))
                    {
                        e.Handled = true;
                        break;
                    }
                }
            }
            else
            {
                if (!this.Multiline)
                {
                    string plusText = UtilString.Substring(this.Text, 0, this.SelectionStart) + e.KeyChar.ToString() + UtilString.Substring(this.Text, this.SelectionStart + this.SelectionLength);
                    if (string.IsNullOrEmpty((new CheckInputRegulation(_isInputRegulation, this.Multiline).StartCheck(e.KeyChar.ToString(), _dict))))
                    {
                        e.Handled = true;
                    }
                    else if (new CheckProphitionChar().StartCheck(plusText, _prohibitionChar) != plusText)
                    {
                        //Console.Beep(400, 100);
                        int start = this.SelectionStart;
                        this.Text = new CheckProphitionChar().StartCheck(plusText, _prohibitionChar);
                        this.Select(start, 0);
                        e.Handled = true;
                    }
                    return;
                }

                // 入力されている文字列の最初から選択されている部分まで
                string beginText = UtilString.Substring(this.Text, 0, this.SelectionStart);
                // 選択されている文字列
                string selectText = e.KeyChar.ToString();
                // 残りの文字列
                string endText = UtilString.Substring(this.Text, this.SelectionStart + this.SelectionLength);

                // 改行文字で入力されている文字列を分解
                string[] textParts = (beginText + selectText + endText).Split(BREAK_CODE, StringSplitOptions.None);

                string adjustText = string.Empty;
                for (int i = 0; i < textParts.Length; i++)
                {
                    // 文字数でチェック
                    if (this.OneLineMaxLength < UtilString.Length(textParts[i]))
                    {
                        e.Handled = true;
                        break;
                    }
                }
            }

            {
                string plusText = UtilString.Substring(this.Text, 0, this.SelectionStart) + e.KeyChar.ToString() + UtilString.Substring(this.Text, this.SelectionStart + this.SelectionLength);
                if (string.IsNullOrEmpty((new CheckInputRegulation(_isInputRegulation, this.Multiline).StartCheck(e.KeyChar.ToString(), _dict))))
                {
                    e.Handled = true;
                }
                else if (new CheckProphitionChar().StartCheck(plusText, _prohibitionChar) != plusText)
                {
                    //Console.Beep(400, 100);
                    int start = this.SelectionStart;
                    this.Text = new CheckProphitionChar().StartCheck(plusText, _prohibitionChar);
                    this.Select(start, 0);
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region MidByte
        /// --------------------------------------------------
        /// <summary>
        /// 文字列の指定されたバイト位置から、指定されたバイト数分の文字列を返す
        /// </summary>
        /// <param name="target">取り出す元になる文字列</param>
        /// <param name="start">取り出しを開始する位置</param>
        /// <param name="byteSize">取り出すバイト数</param>
        /// <returns>指定されたバイト位置から指定されたバイト数分の文字列</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private String MidByte(String target, Int32 start, Int32 byteSize)
        {
            System.Text.Encoding enc = Encoding.GetEncoding(UtilEncoding.SHIFT_JIS);
            byte[] bytes = enc.GetBytes(target);

            string retStr = enc.GetString(bytes, start, byteSize);
            bytes = enc.GetBytes(retStr);
            if (bytes.Length != 0)
            {
                if (bytes[bytes.Length - 1] == 0x45)
                {
                    retStr = enc.GetString(bytes, 0, byteSize - 1);
                }
            }

            return retStr;
        }
        #endregion

        #endregion

        #region パブリックメソッド

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// テキスト内にサロゲート文字が含まれているかどうかを判定します。
        /// </summary>
        /// <returns>含まれていれば True 、含まれていなければ False を返します。</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        public bool IsSurrogate()
        {
            for (int i = 0; i < this.Text.Length; i++)
            {
                if (char.IsSurrogate(this.Text, i))
                {
                    return true;
                }
            }
            return false;
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 文字数を返します。(サロゲート文字を1文字としてカウント)
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        public new int TextLength
        {
            get
            {
                return new System.Globalization.StringInfo(this.Text).LengthInTextElements;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画像情報を取得する
        /// </summary>
        /// <returns>画像情報</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public List<DSWRichTextBoxImageItem> GetImageData()
        {
            var list = new List<DSWRichTextBoxImageItem>();
            GetImageData(this, PastableType != PastableTypeInner.AllObjects, list);
            return list;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画像情報を設定する
        /// </summary>
        /// <param name="list">画像情報</param>
        /// <exception cref="OutOfMemoryException">貼り付け中のメモリ不足</exception>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetImageData(List<DSWRichTextBoxImageItem> list)
        {
            SetImageData(this, PastableType != PastableTypeInner.AllObjects, list);
        }

        /// --------------------------------------------------
        /// <summary>
        /// クリップボード内に画像データがあるか
        /// </summary>
        /// <return>有無</return>
        /// <create>D.Okumura 2019/06/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool HasClipboardDataFromRichText()
        {
            if (Clipboard.ContainsData(DataFormats.Rtf))
            {
                return _globalHasObjectInClipboard && this._hasObjectInClipboard;
            }
            return false;
        }
        #endregion

        #region リッチテキスト関連内部メソッド
        /// --------------------------------------------------
        /// <summary>
        /// データの設定を行います
        /// </summary>
        /// <param name="richText">リッチテキストボックス</param>
        /// <param name="isPastePictureOnly">画像のみを貼り付け</param>
        /// <param name="images">貼り付け画像一覧</param>
        /// <exception cref="OutOfMemoryException">貼り付け中のメモリ不足</exception>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetImageData(RichTextBox richText, bool isPastePictureOnly, IList<DSWRichTextBoxImageItem> images)
        {
            richText.SuspendLayout();
            var lastStart = richText.SelectionStart;
            var lastLength = richText.SelectionLength;
            //System.Diagnostics.Debug.WriteLine(string.Format("start: {0} len:{1}", lastStart, lastLength));
            using (var pf = new System.Diagnostics.PerformanceCounter())
            {
                // 末尾から画像を貼り付ける：オブジェクトを省略した場合、オフセットが狂うことがあるため
                for (var i = images.Count - 1; i >= 0; i--)
                {
                    var item = images[i];
                    richText.Select(lastStart + item.Position, 1);
                    //System.Diagnostics.Debug.WriteLine(string.Format("paste {0} => {1}", i, lastStart + item.Position));
                    // 全ての場合または画像のみの場合、画像を貼り付ける
                    if (!isPastePictureOnly || (isPastePictureOnly && !item.IsObject))
                    {
                        richText.SelectedRtf = TrimRtfPara(item.Rtf);
                    }
                    else
                    {
                        richText.SelectedRtf = string.Empty;
                    }
                }
            }
            richText.SelectionStart = lastStart;
            richText.SelectionLength = lastLength;
            richText.ResumeLayout();
        }


        /// --------------------------------------------------
        /// <summary>
        /// データの取得を行います
        /// </summary>
        /// <param name="richText">リッチテキストボックス</param>
        /// <param name="imageOnly">画像データのみ</param>
        /// <param name="items">画像一覧</param>
        /// <remarks>
        /// 先頭からの画像データを列挙します。
        /// Textの半角スペースを検出して、その位置のRTFデータを取得して、
        /// オブジェクトであるかの判定を行います。
        /// </remarks>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private static void GetImageData(RichTextBox richText, bool imageOnly, IList<DSWRichTextBoxImageItem> items)
        {
            richText.SuspendLayout();
            var lastStart = richText.SelectionStart;
            var lastLength = richText.SelectionLength;
            // 改行位置などの違いがあるため、全体をなめる
            for (var pos = 0; pos < richText.TextLength; pos++)
            {
                richText.SelectionStart = pos;
                richText.SelectionLength = 1;
                switch (richText.SelectionType)
                {
                    case RichTextBoxSelectionTypes.Object:
                        var rtf = TrimRtfPara(richText.SelectedRtf);
                        if (imageOnly && DSWRichTextBoxImageItem.GetRtfIsObject(ref rtf))
                            break;
                        if (items != null)
                        {
                            // リッチテキストのデータを読み出して画像化する
                            var item = new DSWRichTextBoxImageItem()
                            {
                                Rtf = rtf,
                                Position = pos,
                            };
                            items.Add(item);
                            //System.Diagnostics.Debug.WriteLine(string.Format("img pos: {0} len:{1} size:{2} start:{3}", item.Position, rtf.Length, item.CompressedData.Length, lastStart));
                        }
                        break;
                    default:
                        break;
                }
            }
            richText.SelectionStart = lastStart;
            richText.SelectionLength = lastLength;
            richText.ResumeLayout();
        }
        /// --------------------------------------------------
        /// <summary>
        /// RTF末尾のpara制御文を削除します
        /// </summary>
        /// <param name="rtf">データ</param>
        /// <return>整形済みRFTデータ</return>
        /// <remarks>
        /// RTFを直接取得した場合、末尾にパラグラフの開始/終了が入ることがあるため、排除する
        /// </remarks>
        /// <create>D.Okumura 2019/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        static string TrimRtfPara(string rtf)
        {
            var checkEnd = "\\par\r\n}\r\n";
            if (rtf.EndsWith(checkEnd))
                return rtf.Substring(0, rtf.Length - checkEnd.Length) + "}";
            else
                return rtf;
        }

        #endregion
    }
}
