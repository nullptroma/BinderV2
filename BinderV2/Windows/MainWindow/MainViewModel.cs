using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using BinderV2.Trigger.Types;
using BinderV2.Trigger;
using BinderV2.BindModel;
using BinderV2.Commands;
using BinderV2.WpfControls.BindControl;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using BinderV2.Windows.Settings;
using Microsoft.Win32;
using System.Net.Mail;
using BinderV2.Settings;
using System.Windows.Input;

namespace BinderV2
{
    class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IBindElement> bindsControls { get; set; }
        private SettingsWindow SettingsWindow;
        private IBindElement selectedBind;
        private string currentBindScript = "";

        public string CurrentBindScript
        {
            get { return currentBindScript; }
            set
            {
                currentBindScript = value;
                OnPropertyChanged("currentBindScript");
            }
        }

        public MainViewModel()
        {
            bindsControls = new ObservableCollection<IBindElement>();
            if (ProgramSettings.runtimeSettings.AutoLoadBinds)
                try
                {
                    OpenBindsInPath(ProgramSettings.runtimeSettings.AutoLoadBindsPath);
                }
                catch 
                { 
                    MessageBox.Show("Не удаётся открыть файл " + ProgramSettings.runtimeSettings.AutoLoadBindsPath, "Ошибка");
                    ProgramSettings.runtimeSettings.AutoLoadBinds = false; 
                    ProgramSettings.runtimeSettings.AutoLoadBindsPath = ""; 
                }
        }

        private RelayCommand disableTriggersCommand;
        public RelayCommand DisableTriggersCommand
        {
            get
            {
                return disableTriggersCommand ??
                  (disableTriggersCommand = new RelayCommand(obj =>
                  {
                      BaseTrigger.EnableAllTriggers = false;
                  }));
            }
        }

        private RelayCommand enableTriggersCommand;
        public RelayCommand EnableTriggersCommand
        {
            get
            {
                return enableTriggersCommand ??
                  (enableTriggersCommand = new RelayCommand(obj =>
                  {
                      BaseTrigger.EnableAllTriggers = true;
                  }));
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
                      SaveFileDialog sfd = new SaveFileDialog();
                      sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                      if (sfd.ShowDialog().Value)
                      {
                          ProgramSettings.runtimeSettings.LastBindsPath = sfd.FileName;
                          SaveBindsCommand.Execute(null);
                      }
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
                      if (ProgramSettings.runtimeSettings.LastBindsPath.Length != 0)
                      {
                          Bind[] binds = new Bind[bindsControls.Count];
                          for (int i = 0; i < binds.Length; i++)
                              binds[i] = bindsControls[i].GetBind();
                          Utilities.JsonUtilities.SerializeToFile(binds, ProgramSettings.runtimeSettings.LastBindsPath);
                      }
                      else
                          SaveBindsInNewPathCommand.Execute(null);
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
                      OpenFileDialog ofd = new OpenFileDialog();
                      ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                      if (ofd.ShowDialog().Value)
                      {
                          try
                          {
                              ProgramSettings.runtimeSettings.LastBindsPath = ofd.FileName;
                              OpenBindsInPath(ProgramSettings.runtimeSettings.LastBindsPath);
                              OnPropertyChanged("bindsControls");
                          }
                          catch { MessageBox.Show("Не удаётся открыть файл " + ofd.FileName, "Ошибка"); }
                      }
                  }));
            }
        }

        private void OpenBindsInPath(string path)
        {
            ClearBinds();
            Bind[] binds = Utilities.JsonUtilities.Deserialize<Bind[]>(File.ReadAllText(path));
            foreach (Bind b in binds)
                bindsControls.Add(new BindElement(b));
        }

        private void ClearBinds()
        {
            foreach (IBindElement be in bindsControls)
                be.GetBind().Dispose();
            bindsControls.Clear();
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

        private RelayCommand createBindCommand;
        public RelayCommand CreateBindCommand
        {
            get
            {
                return createBindCommand ??
                  (createBindCommand = new RelayCommand(obj =>
                  {
                      bindsControls.Add((IBindElement)new BindElement());
                      OnPropertyChanged("bindsControls");
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
                      if (obj is BindElement)
                      {
                          foreach (IBindElement bindControl in bindsControls)
                              bindControl.Selected = false;
                          IBindElement currentBind = (IBindElement)obj;
                          currentBind.Selected = true;
                          selectedBind = currentBind;

                          CurrentBindScript = selectedBind.GetBind().Script;
                      }
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
                      if (selectedBind != null)
                      {
                          selectedBind.GetBind().Script = obj.ToString();
                          MessageBox.Show("Сохранено");
                      }
                      else
                      {
                          MessageBox.Show("Выберите бинд", "Ошибка");
                      }
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
                      IBindElement bindElement = (IBindElement)obj;
                      if (MessageBox.Show("Удалить бинд \"" + bindElement.GetBind().Name + "\"?", "Вы уверены?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                      {
                          if (!bindsControls.Remove(bindElement))
                              MessageBox.Show("Ошибка удаления");
                          bindElement.GetBind().Dispose();//освобождаем удаляемый бинд
                          OnPropertyChanged("bindsControls");
                          CurrentBindScript = "";
                      }
                  }));
            }
        }

        private RelayCommand openSettingsWindowCommand;
        public RelayCommand OpenSettingsWindowCommand
        {
            get
            {
                return openSettingsWindowCommand ??
                  (openSettingsWindowCommand = new RelayCommand(obj =>
                  {
                      if (SettingsWindow != null)
                          SettingsWindow.Close();
                      SettingsWindow = new SettingsWindow();
                      SettingsWindow.Show();
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
