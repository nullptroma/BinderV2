using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using Trigger.Types;
using Trigger;
using BindModel;
using BinderV2.Commands;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using BinderV2.MVVM.Views;
using Microsoft.Win32;
using System.Net.Mail;
using BinderV2.Settings;
using System.Windows.Input;
using Utilities;
using InterpreterScripts;
using InterpreterScripts.InterpretationScriptData.StandartFunctions;
using InterpreterScripts.FuncAttributes;
using BinderV2.MVVM.Models.MainModels;

namespace BinderV2.MVVM.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region BindsManager
        private readonly BindsManager bindsManager = new BindsManager();
        public ObservableCollection<BindViewModel> Binds { get { return bindsManager.Binds; } }

        public string SelectedBindScript
        {
            get { return bindsManager.SelectedBindScript; }
            set
            {
                bindsManager.SelectedBindScript = value;
                OnPropertyChanged("SelectedBindScript");
            }
        }

        private RelayCommand saveBindsInNewPathCommand;
        public RelayCommand SaveBindsInNewPathCommand
        {
            get
            {
                return saveBindsInNewPathCommand ??
                  (saveBindsInNewPathCommand = new RelayCommand(obj =>
                  {
                      bindsManager.SaveBindsInNewPath();
                  }));
            }
        }

        private RelayCommand saveBindsCommand;
        public RelayCommand SaveBindsCommand
        {
            get
            {
                return saveBindsCommand ??
                  (saveBindsCommand = new RelayCommand(obj =>
                  {
                      bindsManager.SaveBinds();
                  }));
            }
        }

        private RelayCommand openBindsCommand;
        public RelayCommand OpenBindsCommand
        {
            get
            {
                return openBindsCommand ??
                  (openBindsCommand = new RelayCommand(obj =>
                  {
                      bindsManager.OpenBinds();
                  }));
            }
        }

        private RelayCommand selectBindCommand;
        public RelayCommand SelectBindCommand
        {
            get
            {
                return selectBindCommand ??
                  (selectBindCommand = new RelayCommand(obj =>
                  {
                      if (obj is BindViewModel bvm)
                          bindsManager.SelectedBind = bvm;
                  }));
            }
        }

        private RelayCommand saveCurrentScript;
        public RelayCommand SaveCurrentScript
        {
            get
            {
                return saveCurrentScript ??
                  (saveCurrentScript = new RelayCommand(obj =>
                  {
                      if (obj != null)
                      {
                          bindsManager.SaveScriptToSelectedBind(obj.ToString());
                          MessageBox.Show("Сохранено", "Успех");
                      }    
                  }));
            }
        }

        private RelayCommand createBindCommand;
        public RelayCommand CreateBindCommand
        {
            get
            {
                return createBindCommand ??
                  (createBindCommand = new RelayCommand(obj =>
                  {
                      bindsManager.CreateNewBind();
                  }));
            }
        }

        private RelayCommand removeBindCommand;
        public RelayCommand RemoveBindCommand
        {
            get
            {
                return removeBindCommand ??
                  (removeBindCommand = new RelayCommand(obj =>
                  {
                      if (!(obj is BindViewModel))
                          return;
                      BindViewModel bindElement = (BindViewModel)obj;
                      if (MessageBox.Show("Удалить бинд \"" + bindElement.Bind.Name + "\"?", "Вы уверены?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                      {
                          if (!bindsManager.RemoveBind(bindElement))
                              MessageBox.Show("Ошибка удаления");
                      }
                  }));
            }
        }

        private void OnBindsManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
        #endregion

        #region WindowsManager
        private readonly WindowsManager wm = new WindowsManager();

        private RelayCommand openSettingsWindowCommand;
        public RelayCommand OpenSettingsWindowCommand
        {
            get
            {
                return openSettingsWindowCommand ??
                  (openSettingsWindowCommand = new RelayCommand(obj =>
                  {
                      wm.OpenSettingsWindow();
                  }));
            }
        }

        private RelayCommand openHelpWindowCommand;
        public RelayCommand OpenHelpWindowCommand
        {
            get
            {
                return openHelpWindowCommand ??
                  (openHelpWindowCommand = new RelayCommand(obj =>
                  {
                      wm.OpenHelpWindow();
                  }));
            }
        }

        private RelayCommand openRecordWindowCommand;
        public RelayCommand OpenRecordWindowCommand
        {
            get
            {
                return openRecordWindowCommand ??
                  (openRecordWindowCommand = new RelayCommand(obj =>
                  {
                      wm.OpenRecordWindow();
                  }));
            }
        }
        #endregion

        public MainViewModel()
        {
            bindsManager.PropertyChanged += OnBindsManagerPropertyChanged;
        }
        
        private RelayCommand appShutdownCommand;
        public RelayCommand AppShutdownCommand
        {
            get
            {
                return appShutdownCommand ??
                  (appShutdownCommand = new RelayCommand(obj =>
                  {
                      Environment.Exit(0);
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
