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
using System.Windows.Media;
using BinderV2.Settings;
using BinderV2.Settings.Visuals;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.Win32;

namespace BinderV2.MVVM.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        private VisualsSettings currentVsEdit = ProgramSettings.RuntimeSettings.VisualSettings;//ссылка на настройки интерфейса программы

        public static bool StartWithWindows { get { return ProgramSettings.RuntimeSettings.StartWithWindows; } set { ProgramSettings.RuntimeSettings.StartWithWindows = value; } }
        public static bool HideOnStart { get { return ProgramSettings.RuntimeSettings.HideOnStart; } set { ProgramSettings.RuntimeSettings.HideOnStart = value; } }
        public static bool AutoLoadBinds { get { return ProgramSettings.RuntimeSettings.AutoLoadBinds; } set { ProgramSettings.RuntimeSettings.AutoLoadBinds = value; } }
        public static bool SaveMainWindowSize { get { return ProgramSettings.RuntimeSettings.SaveMainWindowSize; } set { ProgramSettings.RuntimeSettings.SaveMainWindowSize = value; } }
        public static string AutoLoadBindsPath { get { return ProgramSettings.RuntimeSettings.AutoLoadBindsPath; } set { ProgramSettings.RuntimeSettings.AutoLoadBindsPath = value; } }

        public ObservableCollection<string> colorFields { get; set; }//все цветные параметры
        public string SelectedColorField { get; set; }//выбранный цветной параметр

        //для редактирования цвета
        public byte currentRed 
        {
            get 
            {
                try { return ((Color)currentVsEdit.GetType().GetRuntimeField(SelectedColorField).GetValue(currentVsEdit)).R; }
                catch { return 0; }
            }
            set 
            {
                UpdateColors(currentAlpha, value, currentGreen, currentBlue);
                OnPropertyChanged("currentRed");
            }
        }
        public byte currentGreen
        {
            get
            {
                try { return ((Color)currentVsEdit.GetType().GetRuntimeField(SelectedColorField).GetValue(currentVsEdit)).G; }
                catch { return 0; }
            }
            set
            {
                UpdateColors(currentAlpha, currentRed, value, currentBlue);
                OnPropertyChanged("currentGreen");
            }
        }
        public byte currentBlue
        {
            get
            {
                try { return ((Color)currentVsEdit.GetType().GetRuntimeField(SelectedColorField).GetValue(currentVsEdit)).B; }
                catch { return 0; }
            }
            set
            {
                UpdateColors(currentAlpha, currentRed, currentGreen, value);
                OnPropertyChanged("currentBlue");
            }
        }
        public byte currentAlpha
        {
            get
            {
                try { return ((Color)currentVsEdit.GetType().GetRuntimeField(SelectedColorField).GetValue(currentVsEdit)).A; }
                catch { return 0; }
            }
            set
            {
                UpdateColors(value, currentRed, currentGreen, currentBlue);
                OnPropertyChanged("currentAlpha");
            }
        }

        private void UpdateColors(byte a, byte r, byte g, byte b)
        {
            currentVsEdit.GetType().GetRuntimeField(SelectedColorField).SetValue(currentVsEdit, Color.FromArgb(a, r, g, b));
            VisualsSettings.ApplyVisuals(currentVsEdit);
        }

        private void NotifyAboutCurrentColors()
        {
            OnPropertyChanged("currentRed");
            OnPropertyChanged("currentGreen");
            OnPropertyChanged("currentBlue");
            OnPropertyChanged("currentAlpha");
        }

        public string WindowBorderThickness 
        { 
            get 
            {
                return currentVsEdit.WindowBorderThickness.ToString(); 
            } 
            set 
            {
                currentVsEdit.WindowBorderThickness = GetThickness(value);
                OnPropertyChanged("WindowBorderThickness");
                VisualsSettings.ApplyVisuals(currentVsEdit);
            }
        }
        public string WindowIconMargin
        {
            get
            {
                return currentVsEdit.WindowIconMargin.ToString();
            }
            set
            {
                currentVsEdit.WindowIconMargin = GetThickness(value);
                OnPropertyChanged("WindowIconMargin");
                VisualsSettings.ApplyVisuals(currentVsEdit);
            }
        }
        public string HeightWindowTitle
        {
            get
            {
                return currentVsEdit.HeightWindowTitle.ToString();
            }
            set
            {
                try
                {
                    currentVsEdit.HeightWindowTitle = double.Parse(value);
                    OnPropertyChanged("HeightWindowTitle");
                    VisualsSettings.ApplyVisuals(currentVsEdit);
                }
                catch { }
            }
        }
        public string WindowIconSize
        {
            get
            {
                return currentVsEdit.WindowIconSize.ToString();
            }
            set
            {
                try
                {
                    currentVsEdit.WindowIconSize = double.Parse(value);
                    OnPropertyChanged("WindowIconSize");
                    VisualsSettings.ApplyVisuals(currentVsEdit);
                }
                catch { }
            }
        }
        public string TitleFontSize
        {
            get
            {
                return currentVsEdit.TitleFontSize.ToString();
            }
            set
            {
                try
                {
                    currentVsEdit.TitleFontSize = double.Parse(value);
                    OnPropertyChanged("TitleFontSize");
                    VisualsSettings.ApplyVisuals(currentVsEdit);
                }
                catch { }
            }
        }

        private Thickness GetThickness(string thickness)
        {
            Thickness answer;
            thickness = thickness.Replace(" ", "");
            double output;
            if (thickness.Count(ch => ch == ',') == 3)
            {
                double[] nums = new double[4];
                string[] numsString = thickness.Split(',');
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        nums[i] = double.Parse(numsString[i]);
                    }
                    catch { throw new FormatException("Wrong format " + thickness); }
                }
                answer = new Thickness(nums[0], nums[1], nums[2], nums[3]);
            }
            else if (double.TryParse(thickness, out output))
            {
                answer = new Thickness(output);
            }
            else
            {
                throw new FormatException("Wrong format " + thickness);
            }
            return answer;
        }

        private RelayCommand resetSettingsCommand;
        public RelayCommand ResetSettingsCommand
        {
            get
            {
                return resetSettingsCommand ??
                  (resetSettingsCommand = new RelayCommand(obj =>
                  {
                      if (MessageBox.Show("Сбросить все настройки?", "Вы уверены?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                      {
                          ProgramSettings.Reset();
                          this.currentVsEdit = ProgramSettings.RuntimeSettings.VisualSettings;
                          OnPropertyChanged("WindowBorderThickness");
                          OnPropertyChanged("WindowIconMargin");
                          OnPropertyChanged("HeightWindowTitle");
                          OnPropertyChanged("WindowIconSize");
                      }
                  }));
            }
        }

        

        private RelayCommand colorFieldChangedCommand;
        public RelayCommand ColorFieldChangedCommand
        {
            get
            {
                return colorFieldChangedCommand ??
                  (colorFieldChangedCommand = new RelayCommand(obj =>
                  {
                      if (SelectedColorField == "None")
                          return;
                      NotifyAboutCurrentColors();
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
                      OpenFileDialog ofd = new OpenFileDialog();
                      ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                      if (ofd.ShowDialog().Value)
                      {
                          ProgramSettings.RuntimeSettings.AutoLoadBindsPath = ofd.FileName;
                          OnPropertyChanged("AutoLoadBindsPath");
                      }
                  },
                  obj => ProgramSettings.RuntimeSettings.AutoLoadBinds));
            }
        }

        public SettingsViewModel()
        {
            OnPropertyChanged("windowBorderThickness");
            SetColorFields();
        }

        private void SetColorFields()
        {
            colorFields = new ObservableCollection<string>();
            colorFields.Add("None");
            SelectedColorField = "None";
            foreach (FieldInfo fi in currentVsEdit.GetType().GetRuntimeFields())
            {
                if (fi.IsPublic)
                    if(fi.FieldType == typeof(Color))
                        colorFields.Add(fi.Name);
            }
            OnPropertyChanged("colorFields");
        }

        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
