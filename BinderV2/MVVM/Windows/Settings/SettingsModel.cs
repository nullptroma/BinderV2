using BinderV2.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BinderV2.Settings.Visuals;
using System.Windows;
using System.CodeDom;
using Microsoft.Win32;

namespace BinderV2.MVVM.Models
{
    class SettingsModel : INotifyPropertyChanged
    {
        private ProgramSettings editSettings = ProgramSettings.RuntimeSettings;

        public bool StartWithWindows 
        { 
            get { return editSettings.StartWithWindows; }
            set 
            { 
                editSettings.StartWithWindows = value;
                if (value)
                    Utilities.AutoRun.RegisterAutoRun();
                else
                    Utilities.AutoRun.UnRegisterAutoRun();
            }
        }
        public bool HideOnStart { get { return editSettings.HideOnStart; } set { editSettings.HideOnStart = value; } }
        public bool AutoLoadBinds { get { return editSettings.AutoLoadBinds; } set { editSettings.AutoLoadBinds = value; } }
        public string AutoLoadBindsPath { get { return editSettings.AutoLoadBindsPath; } private set { editSettings.AutoLoadBindsPath = value;  OnPropertyChanged("AutoLoadBindsPath"); } }
        public bool SaveMainWindowSize { get { return editSettings.SaveMainWindowSize; } set { editSettings.SaveMainWindowSize = value;  OnPropertyChanged("AutoLoadBindsPath"); } }

        public void GetAutoLoadBindsPathFromUser()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (ofd.ShowDialog().Value)
                AutoLoadBindsPath = ofd.FileName;
        }

        #region ColorSettings
        public ObservableCollection<string> ColorParameters { get; private set; }
        private string selectedColorParameter;
        public string SelectedColorParameter
        {
            get { return selectedColorParameter; }
            set 
            { 
                selectedColorParameter = value;
                OnPropertyChanged("SelectedColorParameter");
                NotifyAboutCurrentColors();
            }
        }

        private void NotifyAboutCurrentColors()
        {
            OnPropertyChanged("CurrentRed");
            OnPropertyChanged("CurrentGreen");
            OnPropertyChanged("CurrentBlue");
            OnPropertyChanged("CurrentAlpha");
        }

        public byte CurrentRed
        {
            get
            {
                return GetVisualParameterColor(SelectedColorParameter).R;
            }
            set
            {
                Color c = GetVisualParameterColor(SelectedColorParameter);
                c.R = value;
                SetCurrentColor(c);
            }
        }
        public byte CurrentGreen
        {
            get
            {
                return GetVisualParameterColor(SelectedColorParameter).G;
            }
            set
            {
                Color c = GetVisualParameterColor(SelectedColorParameter);
                c.G = value;
                SetCurrentColor(c);
            }
        }
        public byte CurrentBlue
        {
            get
            {
                return GetVisualParameterColor(SelectedColorParameter).B;
            }
            set
            {
                Color c = GetVisualParameterColor(SelectedColorParameter);
                c.B = value;
                SetCurrentColor(c);
            }
        }
        public byte CurrentAlpha
        {
            get
            {
                return GetVisualParameterColor(SelectedColorParameter).A;
            }
            set
            {
                Color c = GetVisualParameterColor(SelectedColorParameter);
                c.A = value;
                SetCurrentColor(c);
            }
        }

        private Color GetVisualParameterColor(string name)
        {
            Type t = typeof(VisualsSettings);
            FieldInfo parameter = t.GetRuntimeField(name);
            return parameter != null ? (Color)parameter.GetValue(editSettings.VisualSettings) : Color.FromArgb(0, 0, 0, 0);
        }

        private void SetCurrentColor(Color c)
        {
            FieldInfo toEdit = typeof(VisualsSettings).GetRuntimeField(SelectedColorParameter);
            toEdit?.SetValue(editSettings.VisualSettings, c);
            VisualsSettings.ApplyVisuals(editSettings.VisualSettings);
        }

        private void GetColorParameters()
        {
            ColorParameters = new ObservableCollection<string>
            {
                "None"
            };
            SelectedColorParameter = "None";
            foreach (FieldInfo fi in editSettings.VisualSettings.GetType().GetRuntimeFields())
            {
                if (fi.IsPublic)
                    if (fi.FieldType == typeof(Color))
                        ColorParameters.Add(fi.Name);
            }
        }
        #endregion
        #region TextParameters
        public ObservableCollection<string> TextParameters { get; private set; }
        private string selectedTextParameter;
        public string SelectedTextParameter
        {
            get { return selectedTextParameter; }
            set
            {
                selectedTextParameter = value;
                OnPropertyChanged("SelectedTextParameter");
                OnPropertyChanged("CurrentTextValue");
            }
        }
        public string CurrentTextValue 
        {
            get { return SelectedTextParameter != "None" ? typeof(VisualsSettings).GetRuntimeField(SelectedTextParameter).GetValue(editSettings.VisualSettings).ToString() : ""; }
            set 
            {
                if (SelectedTextParameter == "None")
                    return;

                //тип параметра
                string typeName = typeof(VisualsSettings).GetRuntimeField(SelectedTextParameter).FieldType.Name;
                FieldInfo field = typeof(VisualsSettings).GetRuntimeField(SelectedTextParameter);
                switch (typeName.ToLower())
                {
                    case "double":
                        if (double.TryParse(value, out double result))
                            field.SetValue(editSettings.VisualSettings, result);
                        break;
                    case "thickness":
                        try { field.SetValue(editSettings.VisualSettings, GetThickness(value)); }
                        catch { MessageBox.Show("Для Thickness задан неверный формат.", "Ошибка"); }
                        break;
                }
                VisualsSettings.ApplyVisuals(editSettings.VisualSettings);
            }
        }

        private Thickness GetThickness(string thickness)
        {
            Thickness answer;
            thickness = thickness.Replace(" ", "");
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
            else if (double.TryParse(thickness, out double output))
                answer = new Thickness(output);
            else
                throw new FormatException("Wrong format " + thickness);
            return answer;
        }

        private void GetTextParameters()
        {
            TextParameters = new ObservableCollection<string>() { "None" };
            SelectedTextParameter = "None";
            foreach (FieldInfo fi in editSettings.VisualSettings.GetType().GetRuntimeFields())
            {
                if (fi.IsPublic)
                    if (fi.FieldType != typeof(Color))
                        TextParameters.Add(fi.Name);
            }
        }
        #endregion

        public void Reset()
        {
            ProgramSettings.Reset();
            editSettings = ProgramSettings.RuntimeSettings;
            NotifyAboutCurrentColors();
            OnPropertyChanged("SelectedTextParameter");
            OnPropertyChanged("CurrentTextValue");
        }

        public SettingsModel()
        {
            GetColorParameters();
            GetTextParameters();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
