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
using System.Windows.Media;
using BinderV2.Settings;
using BinderV2.Settings.Visuals;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.Win32;

namespace BinderV2.Windows.SettingsWindow
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private VisualsSettings currentVsEdit = ProgramSettings.runtimeSettings.VisualSettings;//ссылка на настройки интерфейса программы

        public static bool StartWithWindows { get { return ProgramSettings.runtimeSettings.StartWithWindows; } set { ProgramSettings.runtimeSettings.StartWithWindows = value; } }
        public static bool HideOnStart { get { return ProgramSettings.runtimeSettings.HideOnStart; } set { ProgramSettings.runtimeSettings.HideOnStart = value; } }
        public static bool AutoLoadBinds { get { return ProgramSettings.runtimeSettings.AutoLoadBinds; } set { ProgramSettings.runtimeSettings.AutoLoadBinds = value; } }
        public static string AutoLoadBindsPath { get { return ProgramSettings.runtimeSettings.AutoLoadBindsPath; } set { ProgramSettings.runtimeSettings.AutoLoadBindsPath = value; } }

        public ObservableCollection<string> colorFields { get; set; }//все цветные параметры
        public string SelectedColorField { get; set; }//выбранный цветной параметр

        //для редактирования цвета
        public byte currentRed { get; set; }
        public byte currentGreen { get; set; }
        public byte currentBlue { get; set; }
        public byte currentAlpha { get; set; }

        //для редактирования других полей
        public string windowBorderThickness { get; set; }

        private RelayCommand applyTextSettingsCommand;//команда для обновления всех текстовых параметров
        public RelayCommand ApplyTextSettingsCommand
        {
            get
            {
                return applyTextSettingsCommand ??
                  (applyTextSettingsCommand = new RelayCommand(obj =>
                  {
                      UpdateBorderThickness();
                      VisualsSettings.ApplyVisuals(currentVsEdit);
                      MessageBox.Show("Если ошибок не было, значит сохранено", "Успех");
                  }));
            }
        }

        private void UpdateBorderThickness()
        {
            windowBorderThickness = windowBorderThickness.Replace(" ", "");
            double output;
            if (windowBorderThickness.Count(ch => ch == ',') == 3)
            {
                double[] nums = new double[4];
                string[] numsString = windowBorderThickness.Split(',');
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        nums[i] = double.Parse(numsString[i]);
                    }
                    catch { MessageBox.Show("windowBorderThickness в неверном формате", "Ошибка"); break; }
                    currentVsEdit.WindowBorderThickness = new Thickness(nums[0], nums[1], nums[2], nums[3]);
                }
            }
            else if (double.TryParse(windowBorderThickness, out output))
            {
                currentVsEdit.WindowBorderThickness = new Thickness(output);
            }
            else
            {
                MessageBox.Show("windowBorderThickness в неверном формате", "Ошибка");
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
                      Color fieldColor = (Color)currentVsEdit.GetType().GetRuntimeField(SelectedColorField).GetValue(currentVsEdit);
                      currentRed = fieldColor.R;
                      currentGreen = fieldColor.G;
                      currentBlue = fieldColor.B;
                      currentAlpha = fieldColor.A;

                      OnPropertyChanged("currentRed");
                      OnPropertyChanged("currentGreen");
                      OnPropertyChanged("currentBlue");
                      OnPropertyChanged("currentAlpha");
                  }));
            }
        } 

        private RelayCommand colorChangedCommand;
        public RelayCommand ColorChangedCommand
        {
            get
            {
                return colorChangedCommand ??
                  (colorChangedCommand = new RelayCommand(obj =>
                  {
                      switch (obj.ToString())
                      {
                          case "Red":
                              {
                                  OnPropertyChanged("currentRed");
                                  break;
                              }
                          case "Green":
                              {
                                  OnPropertyChanged("currentGreen");
                                  break;
                              }
                          case "Blue":
                              {
                                  OnPropertyChanged("currentBlue");
                                  break;
                              }
                          case "Alpha":
                              {
                                  OnPropertyChanged("currentAlpha");
                                  break;
                              }
                      }
                      if (SelectedColorField != "None")
                      {
                          currentVsEdit.GetType().GetRuntimeField(SelectedColorField).SetValue(currentVsEdit, Color.FromArgb(currentAlpha, currentRed, currentGreen, currentBlue));
                          VisualsSettings.ApplyVisuals(currentVsEdit);
                      }
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
                          ProgramSettings.runtimeSettings.AutoLoadBindsPath = ofd.FileName;
                          OnPropertyChanged("AutoLoadBindsPath");
                      }
                  },
                  obj => ProgramSettings.runtimeSettings.AutoLoadBinds));
            }
        }

        public SettingsViewModel()
        {
            windowBorderThickness = currentVsEdit.WindowBorderThickness.ToString();
            OnPropertyChanged("windowBorderThickness");
            SetFields();
        }

        private void SetFields()
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
            OnPropertyChanged("textFields");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
