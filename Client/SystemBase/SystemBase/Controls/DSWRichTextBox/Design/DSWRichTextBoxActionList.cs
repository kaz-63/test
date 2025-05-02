using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing;
using DSWControl.DSWRichTextBox;

namespace DSWControl.DSWRichTextBox.Design
{
    /// ---------------------------------------------------------------------------
    /// <summary>
    /// DSWRichTextBoxのスマートタグパネル作成クラス
    /// </summary>
    /// <create>D.Okumura 2019/06/13</create>
    /// <update></update>
    /// ---------------------------------------------------------------------------
    internal class DSWRichTextBoxActionList : DesignerActionList
    {
        #region フィールド
        /// --------------------------------------------------
        /// <summary>
        /// DSWRichTextBoxを保持するフィールド
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private global::DSWControl.DSWRichTextBox.DSWRichTextBox _ctrlTextBox;

        /// --------------------------------------------------
        /// <summary>
        /// スマート タグ パネルのユーザー インターフェイス (UI) を管理します。
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private DesignerActionUIService _designerActionUISvc = null;
        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="component">デザイナで操作する対象のコンポーネント。</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public DSWRichTextBoxActionList(IComponent component)
            : base(component)
        {

            this._ctrlTextBox = component as global::DSWControl.DSWRichTextBox.DSWRichTextBox;

            this._designerActionUISvc = GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
        }

        #endregion

        #region プロパティ取得

        /// --------------------------------------------------
        /// <summary>
        /// プロパティの取得
        /// </summary>
        /// <param name="propName">プロパティ名</param>
        /// <returns>対象プロパティ</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private PropertyDescriptor GetPropertyByName(String propName)
        {
            PropertyDescriptor prop;
            prop = TypeDescriptor.GetProperties(this._ctrlTextBox)[propName];
            if (null == prop)
            {
                throw new ArgumentException("Matching DSWRichTextBoxActionList property not found", propName);
            }
            else
            {
                return prop;
            }
        }

        #endregion

        #region プロパティ

        #region 表示

        /// --------------------------------------------------
        /// <summary>
        /// コントロールのテキストの内容を取得または設定します。
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Editor("System.ComponentModel.Design.MultilineStringEditor", typeof(System.Drawing.Design.UITypeEditor))]
        public string Text
        {
            get { return this._ctrlTextBox.Text; }
            set
            {
                this.GetPropertyByName("Text").SetValue(this._ctrlTextBox, value);
                // ユーザーインターフェイスのリフレッシュ
                this._designerActionUISvc.Refresh(this.Component);
            }
        }
        #endregion

        #region カスタム表示
        /// ---------------------------------------------------------------------------
        /// <summary>
        /// フォーカス時のバックカラーの取得、設定
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        public Color FocusBackColor
        {
            get { return this._ctrlTextBox.FocusBackColor; }
            set { this.GetPropertyByName("FocusBackColor").SetValue(this._ctrlTextBox, value); }
        }
        #endregion

        #region カスタム動作

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 入力規制の取得、設定
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        [Editor(typeof(InputRegulationEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public string InputRegulation
        {
            get { return this._ctrlTextBox.InputRegulation; }
            set { this.GetPropertyByName("InputRegulation").SetValue(this._ctrlTextBox, value); }
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 入力規制にセットしたものを使用可にするのか、不可にするのか
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        public bool IsInputRegulation
        {
            get { return this._ctrlTextBox.IsInputRegulation; }
            set { this.GetPropertyByName("IsInputRegulation").SetValue(this._ctrlTextBox, value); }
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 入力最大長がbyteか文字数か
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        public bool MaxByteLengthMode
        {
            get { return this._ctrlTextBox.MaxByteLengthMode; }
            set { this.GetPropertyByName("MaxByteLengthMode").SetValue(this._ctrlTextBox, value); }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 禁止文字の設定
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        [Editor(typeof(ProhibitionCharEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public System.Collections.Generic.Dictionary<int, string> ProhibitionChar
        {
            get { return this._ctrlTextBox.ProhibitionChar; }
            set { this.GetPropertyByName("ProhibitionChar").SetValue(this._ctrlTextBox, value); }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 何行入力できるか？
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public int MaxLineCount
        {
            get { return this._ctrlTextBox.MaxLineCount; }
            set { this.GetPropertyByName("MaxLineCount").SetValue(this._ctrlTextBox, value); }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 一行に入力可能な文字数
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public int OneLineMaxLength
        {
            get { return this._ctrlTextBox.OneLineMaxLength; }
            set
            {
                if (value < 0)
                {
                    DSWUtil.MsgBoxEx.ShowError("OneLineMaxLength に 0 より小さな値を設定することはできません。");
                    return;
                }
                if (this._ctrlTextBox.MaxLength < value)
                {
                    DSWUtil.MsgBoxEx.ShowError("OneLineMaxLength に MaxLength より大きな値を設定することはできません。");
                    return;
                }
                this.GetPropertyByName("OneLineMaxLength").SetValue(this._ctrlTextBox, value);
            }
        }

        #endregion

        #endregion

        #region スマートタグパネルのユーザー インターフェイス (UI) の項目作成

        /// --------------------------------------------------
        /// <summary>
        /// スマートタグパネルのユーザー インターフェイス (UI) の項目作成
        /// </summary>
        /// <returns>スマートタグパネルの項目</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();

            // スマートタグパネルのヘッダ作成
            items.Add(new DesignerActionHeaderItem("表示"));
            items.Add(new DesignerActionHeaderItem("カスタム表示"));
            items.Add(new DesignerActionHeaderItem("カスタム動作"));

            // 表示のプロパティ作成
            items.Add(new DesignerActionPropertyItem("Text",
                            "テキスト", "表示",
                            "コントロールに関連付けられたテキスト"));

            // カスタム表示のプロパティ作成
            items.Add(new DesignerActionPropertyItem("FocusBackColor",
                            "フォーカス時の背景色", "カスタム表示",
                            "フォーカスが当たったときの背景色の設定を行います。"));

            // カスタム動作のプロパティ作成
            items.Add(new DesignerActionPropertyItem("InputRegulation",
                            "入力規制の設定", "カスタム動作",
                            "入力規制の設定を行います。\r\n選択した種類が対象となります。\r\n一つも選択しない場合はすべて使える状態になります。"));

            items.Add(new DesignerActionPropertyItem("IsInputRegulation",
                            "入力規制に設定したものを使用可にする", "カスタム動作",
                            "選択をした場合、入力規制の対象が使用可能になります。\r\n選択をしない場合、入力規制の対象以外が使用可能になります。"));

            items.Add(new DesignerActionPropertyItem("MaxByteLengthMode",
                            "入力最大長をbyteで検証する", "カスタム動作",
                            "選択をすると入力最大長をbyteで検証します。\r\n選択をしない場合は従来どおり文字数で検証します。"));

            items.Add(new DesignerActionPropertyItem("ProhibitionChar",
                            "禁止文字の設定", "カスタム動作",
                            "禁止文字の設定を行います。\r\n指定した文字列が対象となります。\r\n一つも指定しない場合は禁止される文字はありません。"));

            items.Add(new DesignerActionPropertyItem("MaxLineCount",
                            "入力できる行数", "カスタム動作",
                            "入力できる行数の設定を行います。\r\n0を設定した場合は、行数制限は無くなります。"));

            items.Add(new DesignerActionPropertyItem("OneLineMaxLength",
                            "一行に入力可能な文字数", "カスタム動作",
                            "一行に入力可能な文字数の設定を行います。\r\nMaxLengthより大きい値は設定できません。"));

            return items;
        }
        #endregion
    }
}
