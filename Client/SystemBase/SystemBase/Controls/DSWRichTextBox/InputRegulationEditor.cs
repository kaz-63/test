using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;

namespace DSWControl.DSWRichTextBox
{
    /// --------------------------------------------------
    /// <summary>
    /// PropertyGridに[...]を表示し、自作フォームを出すための設定
    /// </summary>
    /// <create>sakiori 2008/07/02</create>
    /// <update></update>
    /// --------------------------------------------------
    class InputRegulationEditor : UITypeEditor
    {
        /// ---------------------------------------------------------------------------
        /// <summary>
        /// EditValue メソッドで使用するエディタ スタイルを取得します。
        /// </summary>
        /// <param name="context">追加のコンテキスト情報を取得するために使用できる ITypeDescriptorContext。</param>
        /// <returns></returns>
        /// <create>[2008/12/17] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
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

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// GetEditStyle メソッドで提供されたエディタ スタイルを使用して、指定したオブジェクトの値を編集します。
        /// </summary>
        /// <param name="context">追加のコンテキスト情報を取得するために使用できる ITypeDescriptorContext。</param>
        /// <param name="provider">このエディタがサービスを取得するために使用できる IServiceProvider。</param>
        /// <param name="value">編集対象のオブジェクト。</param>
        /// <returns>オブジェクトの新しい値。オブジェクトの値が変更されていない場合は、このメソッドは渡されたオブジェクトと同じオブジェクトを返します。</returns>
        /// <create>[2008/12/17] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService es = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            InputRegulationSetting dlg;
            DSWRichTextBox dswTextBox = context.Instance as DSWRichTextBox;
            if (dswTextBox == null)
            {
                DSWControl.DSWRichTextBox.Design.DSWRichTextBoxActionList actList = context.Instance as DSWControl.DSWRichTextBox.Design.DSWRichTextBoxActionList;
                if (actList != null)
                {
                    dswTextBox = actList.Component as DSWRichTextBox;
                    if (dswTextBox == null)
                    {
                        return value;
                    }
                }
                else
                {
                    return value;
                }
            }
            bool enableIsInputRegulation = true;

            if (context.Instance.GetType().Equals(typeof(global::DSWControl.DSWRichTextBox.Design.DSWRichTextBoxActionList)) == true)
            {
                enableIsInputRegulation = false;
            }

            dlg = new InputRegulationSetting(enableIsInputRegulation);
            dlg.InputRegulation = value.ToString();

            dlg.IsInputRegulation = dswTextBox.IsInputRegulation;
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            if (es.ShowDialog(dlg) == System.Windows.Forms.DialogResult.OK)
            {
                PropertyDescriptor prop;
                prop = TypeDescriptor.GetProperties(dswTextBox)["IsInputRegulation"];
                if (prop != null)
                {
                    prop.SetValue(dswTextBox, dlg.IsInputRegulation);
                }

                return dlg.InputRegulation;
            }
            return value;
        }

    }
}
