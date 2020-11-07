using System.Collections.ObjectModel;
using System.ComponentModel;
using BinderV2.Commands;
using BinderV2.MVVM.Models;

namespace BinderV2.MVVM.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        private readonly SettingsModel model = new SettingsModel();

        public bool StartWithWindows { get { return model.StartWithWindows; } set { model.StartWithWindows = value; } }
        public bool HideOnStart { get { return model.HideOnStart; } set { model.HideOnStart = value; } }
        public bool AutoLoadBinds { get { return model.AutoLoadBinds; } set { model.AutoLoadBinds = value; } }
        public string AutoLoadBindsPath { get { return model.AutoLoadBindsPath; } }
        public bool SaveMainWindowSize { get { return model.SaveMainWindowSize; } set { model.SaveMainWindowSize = value; } }
        public bool CloseEqualsHide { get { return model.CloseEqualsHide; } set { model.CloseEqualsHide = value; } }

        public ObservableCollection<string> TextParameters { get { return model.TextParameters; } }
        public string SelectedTextParameter { get { return model.SelectedTextParameter; } set { model.SelectedTextParameter = value; } }
        public string CurrentTextValue { get { return model.CurrentTextValue; } set { model.CurrentTextValue = value; } }

        public ObservableCollection<string> ColorParameters { get { return model.ColorParameters; } }
        public string SelectedColorParameter { get { return model.SelectedColorParameter; } set { model.SelectedColorParameter = value; } }

        public byte CurrentRed { get { return model.CurrentRed; } set { model.CurrentRed = value; } }
        public byte CurrentGreen { get { return model.CurrentGreen; } set { model.CurrentGreen = value; } }
        public byte CurrentBlue { get { return model.CurrentBlue; } set { model.CurrentBlue = value; } }
        public byte CurrentAlpha { get { return model.CurrentAlpha; } set { model.CurrentAlpha = value; } }

        private void OnSettingsModelPropertyChanded(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        private RelayCommand resetSettingsCommand;//команда ресета
        public RelayCommand ResetSettingsCommand
        {
            get
            {
                return resetSettingsCommand ??
                  (resetSettingsCommand = new RelayCommand(obj =>
                  {
                      model.Reset();
                  }));
            }
        }

        private RelayCommand chooseAutoLoadBindsPathCommands;//команда для выбора пути для автозагрузки биндов
        public RelayCommand ChooseAutoLoadBindsPathCommands
        {
            get
            {
                return chooseAutoLoadBindsPathCommands ??
                  (chooseAutoLoadBindsPathCommands = new RelayCommand(obj =>
                  {
                      model.GetAutoLoadBindsPathFromUser();
                  },
                  obj => model.AutoLoadBinds));
            }
        }

        public SettingsViewModel()
        {
            model.PropertyChanged += OnSettingsModelPropertyChanded;
        }


        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
