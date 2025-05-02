using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Windows.Forms.Design;

namespace DSWControl.DSWRichTextBox.Design
{
    /// ---------------------------------------------------------------------------
    /// <summary>
    /// DSWRichTextBoxのデザイナ
    /// </summary>
    /// <create>D.Okumura 2019/06/13</create>
    /// <update></update>
    /// ---------------------------------------------------------------------------
    internal class DSWRichTextBoxDesigner : ControlDesigner
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// スマートタグを追加するアクションコレクション
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private DesignerActionListCollection _actionLists;

        /// --------------------------------------------------
        /// <summary>
        /// DSWFunctionButtonを保持するフィールド
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private global::DSWControl.DSWRichTextBox.DSWRichTextBox _ctrlTextBox;

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// 指定したコンポーネントをデザイナで表示、編集、デザインできるように準備します。
        /// </summary>
        /// <param name="component">デザイナで操作する対象のコンポーネント。</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            this._ctrlTextBox = component as global::DSWControl.DSWRichTextBox.DSWRichTextBox;
        }

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// デザイナに関連付けられているコンポーネントで
        /// サポートされているデザイン時アクション リストを取得します。
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (null == _actionLists)
                {
                    //DSWFunctionButtonのスマートタグを作成
                    _actionLists = new DesignerActionListCollection();
                    _actionLists.Add(new global::DSWControl.DSWRichTextBox.Design.DSWRichTextBoxActionList(this.Component));
                }
                return _actionLists;
            }
        }

        #endregion

        #region Dispose

        /// --------------------------------------------------
        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion
    }
}
