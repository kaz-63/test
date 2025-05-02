using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DSWControl.DSWRichTextBox
{
    /// --------------------------------------------------
    /// <summary>
    /// PropertyGridに[...]を表示し、自作フォームを出すための設定
    /// </summary>
    /// <create>T.Sakiori 2008/12/19</create>
    /// <update></update>
    /// --------------------------------------------------
    class ProhibitionCharEditor : UITypeEditor
    {
        /// --------------------------------------------------
        /// <summary>
        /// EditValue メソッドで使用するエディタ スタイルを取得します。
        /// </summary>
        /// <param name="context">追加のコンテキスト情報を取得するために使用できる ITypeDescriptorContext。</param>
        /// <returns></returns>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            else
            {
                return base.GetEditStyle(context);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// GetEditStyle メソッドで提供されたエディタ スタイルを使用して、指定したオブジェクトの値を編集します。
        /// </summary>
        /// <param name="context">追加のコンテキスト情報を取得するために使用できる ITypeDescriptorContext。</param>
        /// <param name="provider">このエディタがサービスを取得するために使用できる IServiceProvider。</param>
        /// <param name="value">編集対象のオブジェクト。</param>
        /// <returns>オブジェクトの新しい値。オブジェクトの値が変更されていない場合は、このメソッドは渡されたオブジェクトと同じオブジェクトを返します。</returns>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService es = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            ProhibitionCharSetting dlg = new ProhibitionCharSetting();

            dlg.ProhibitionChar = (Dictionary<int, string>)value;

            dlg.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            if (es.ShowDialog(dlg) == System.Windows.Forms.DialogResult.OK)
            {
                return dlg.ProhibitionChar;
            }
            return value;
        }
    }
}
