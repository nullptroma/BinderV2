using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using BinderV2.Commands;
using BinderV2.MVVM.Views;
using BindModel;

namespace BinderV2.MVVM.ViewModels
{
    public class BindViewModel : BaseViewModel
    {
        public string Name { get { return Bind.Name; } set { Bind.Name = value; OnPropertyChanged("Name"); } }
        private bool isSelected = false;
        public bool IsSelected { get { return isSelected; } set { isSelected = value; OnPropertyChanged("IsSelected");  } }
        public bool IsEnabled { get { return Bind.Enable; } set { Bind.Enable = value; OnPropertyChanged("IsEnabled");  } }
        public Bind Bind { get; private set; }

        public BindViewModel(Bind b)
        {
            Bind = b;
        }

        public BindViewModel()
        {
            Bind = new Bind();
        }

        private TriggersEditWindow currentTriggersEdit;
        private RelayCommand openTriggersEditCommand;
        public RelayCommand OpenTriggersEditCommand
        {
            get
            {
                return openTriggersEditCommand ??
                  (openTriggersEditCommand = new RelayCommand(obj =>
                  {
                      if (currentTriggersEdit != null)//Если окно уже было запущено, закрываем его
                          currentTriggersEdit.Close();
                      currentTriggersEdit = new TriggersEditWindow(Bind);
                      currentTriggersEdit.Show();
                  }));
            }
        }

        private RelayCommand changeEnableCommand;
        public RelayCommand ChangeEnableCommand
        {
            get
            {
                return changeEnableCommand ??
                  (changeEnableCommand = new RelayCommand(obj =>
                  {
                      IsEnabled = !IsEnabled;
                  }));
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
