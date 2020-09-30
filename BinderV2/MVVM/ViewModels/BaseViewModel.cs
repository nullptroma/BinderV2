using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.MVVM.ViewModels
{
    abstract class BaseViewModel : INotifyPropertyChanged
    {
        public abstract event PropertyChangedEventHandler PropertyChanged;
        public abstract void OnPropertyChanged(string prop);
    }
}
