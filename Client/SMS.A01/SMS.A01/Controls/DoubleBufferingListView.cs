using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SMS.A01.Controls
{
    partial class DoubleBufferingListView : System.Windows.Forms.ListView
    {
        protected override bool DoubleBuffered
        {
            get
            {
                return true;
            }

            set
            {
                //base.DoubleBuffered = value;
            }
        }
        public DoubleBufferingListView() : base() { }
    }
}
