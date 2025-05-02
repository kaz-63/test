using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SystemBase.Controls
{
    public class MonthPicker : DateTimePicker
    {
        // デフォルトの Format と CustomFormat を設定
        public MonthPicker()
        {
            this.Format = DateTimePickerFormat.Custom;
            this.CustomFormat = "yyyy/MM";

            // 初期値を設定
            SetSelectedMonthFirstDay();
        }

        // カスタムメッセージと関連する定数の定義
        private const int WM_NOTIFY = 0x004e;
        private const int DTM_CLOSEMONTHCAL = 0x1000 + 13;
        private const int DTM_GETMONTHCAL = 0x1000 + 8;
        private const int MCM_SETCURRENTVIEW = 0x1000 + 32;
        private const int MCN_VIEWCHANGE = -750;
        private const int MCN_SELECT = -749; 

        // WndProc メソッドをオーバーライドしてカスタム処理を実行
        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WM_NOTIFY)
                {
                    // WM_NOTIFY メッセージを処理
                    var nmhdr = (NMHDR)Marshal.PtrToStructure(m.LParam, typeof(NMHDR));
                    switch (nmhdr.code)
                    {
                        // ポップアップ表示を検出してビューを月の選択に切り替える
                        case -950:
                            {
                                var cal = SendMessage(Handle, DTM_GETMONTHCAL, IntPtr.Zero, IntPtr.Zero);
                                SendMessage(cal, MCM_SETCURRENTVIEW, IntPtr.Zero, (IntPtr)1);
                                break;
                            }

                        // 月の選択を検出してポップアップを閉じる
                        case MCN_VIEWCHANGE:
                            {
                                var nmviewchange = (NMVIEWCHANGE)Marshal.PtrToStructure(m.LParam, typeof(NMVIEWCHANGE));
                                if (nmviewchange.dwOldView == 1 && nmviewchange.dwNewView == 0)
                                {
                                    SendMessage(Handle, DTM_CLOSEMONTHCAL, IntPtr.Zero, IntPtr.Zero);
                                    SetSelectedMonthFirstDay(); // 月の選択が変更されたら初日を設定
                                }

                                break;
                            }
                    }
                }

                base.WndProc(ref m);
            }
            catch
            {
 
            }
        }

        // SendMessage 関数の宣言
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        // NMHDR 構造体の定義
        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int code;
        }

        // NMVIEWCHANGE 構造体の定義
        [StructLayout(LayoutKind.Sequential)]
        private struct NMVIEWCHANGE
        {
            public NMHDR nmhdr;
            public uint dwOldView;
            public uint dwNewView;
        }

        // 選択された月の初日に設定するメソッド
        private void SetSelectedMonthFirstDay()
        {
            var selectedDate = this.Value;
            this.Value = new DateTime(selectedDate.Year, selectedDate.Month, 1);
        }

    }
}
