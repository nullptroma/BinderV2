using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using BinderV2.Commands;
using BinderV2.MVVM.Models.MainModels;

namespace BinderV2.MVVM.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region BindsManager
        private readonly BindsManager bindsManager = new BindsManager();
        public ObservableCollection<BindViewModel> Binds { get { return bindsManager.Binds; } }

        public string SelectedBindScript { get; private set; }
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
                          try 
                          {
                              bindsManager.SaveScriptToSelectedBind(obj.ToString());
                              System.Windows.MessageBox.Show("Сохранено", "Успех");
                          }
                          catch { System.Windows.MessageBox.Show("Выберите бинд", "Ошибка"); }
                          
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
                      if (System.Windows.MessageBox.Show("Удалить бинд \"" + bindElement.Bind.Name + "\"?", "Вы уверены?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                      {
                          if (!bindsManager.RemoveBind(bindElement))
                              System.Windows.MessageBox.Show("Ошибка удаления");
                      }
                  }));
            }
        }
        

        private RelayCommand clearScriptTextBox;
        public RelayCommand ClearScriptTextBox
        {
            get
            {
                return clearScriptTextBox ??
                  (clearScriptTextBox = new RelayCommand(obj =>
                  {
                      SelectedBindScript = "";
                      OnPropertyChanged("SelectedBindScript");
                  }));
            }
        }

        private RelayCommand pasteToScriptTextBox;
        public RelayCommand PasteToScriptTextBox
        {
            get
            {
                return pasteToScriptTextBox ??
                  (pasteToScriptTextBox = new RelayCommand(obj =>
                  {
                      SelectedBindScript = Clipboard.GetText();
                      OnPropertyChanged("SelectedBindScript");
                  }));
            }
        }

        private void OnBindsManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedBindScript")
                SelectedBindScript = bindsManager.SelectedBindScript;
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
        private RelayCommand editDefaultGlobalScript;
        public RelayCommand OpenEditDefaultGlobalScriptWindow
        {
            get
            {
                return editDefaultGlobalScript ??
                  (editDefaultGlobalScript = new RelayCommand(obj =>
                  {
                      wm.OpenEditDefaultGlobalScriptWindow();
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
                      App.Current.Shutdown();
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
