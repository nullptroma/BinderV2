using System;

namespace BinderV2.MVVM.Models.BindModel.Events
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
