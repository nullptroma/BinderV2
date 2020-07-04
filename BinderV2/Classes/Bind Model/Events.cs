using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BinderV2.BindModel.Events
{
    public class EnableBindChangedEventArgs : EventArgs
    {
        public bool Enable { get; set; }
        public EnableBindChangedEventArgs(bool e) : base() 
        {
            Enable = e;
        }
    }

    public delegate void EnableBindChangedEventHandler(object sender, EnableBindChangedEventArgs e);
}
