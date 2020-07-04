using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BinderV2.Settings.Visuals
{
    public class VisualsSettings
    {
        public Thickness WindowBorderThickness = new Thickness(2);
        public Color TitleBarBackgroundBrush = (Color)ColorConverter.ConvertFromString("#EFEFF2");
        public Color WindowBackground = (Color)ColorConverter.ConvertFromString("#EFEFF2");
        public Color WindowBorderBrush = (Color)ColorConverter.ConvertFromString("#747474");
        public Color WindowBorderBrushInactive = (Color)ColorConverter.ConvertFromString("#999999");
        public Color WindowStatusForeground = (Color)ColorConverter.ConvertFromString("#FFFFFF");
        public Color WindowStatusForegroundInactive = (Color)ColorConverter.ConvertFromString("#FFFFFF");
        public Color BorderColors = (Color)ColorConverter.ConvertFromString("#FFB4B4B4");
        public Color TextBoxColor = (Color)ColorConverter.ConvertFromString("#FFEFEFF2");
        public Color ButtonsBackground = (Color)ColorConverter.ConvertFromString("#FFDDDDDD");
        public Color TextColor = (Color)ColorConverter.ConvertFromString("#FF000000");
        public Color TextBoxSelectionColor = (Color)ColorConverter.ConvertFromString("#FF0078D7");
        public Color BindTriggerBackgroundUnSelected = (Color)ColorConverter.ConvertFromString("#FFB9B9B9");
        public Color BindTriggerBorderUnSelected = (Color)ColorConverter.ConvertFromString("#FF616161");
        public Color BindTriggerBorderSelected = (Color)ColorConverter.ConvertFromString("#FF328232");
        public Color BindTriggerBackgroundSelected = (Color)ColorConverter.ConvertFromString("#FFB9C3B9");
        public Color ScrollViewerColor = (Color)ColorConverter.ConvertFromString("#FFF0F0F0");
        public Color ComboboxBackground = (Color)ColorConverter.ConvertFromString("#EFEFF2");
        public Color BackgroundBrushPopUpComboBox = (Color)ColorConverter.ConvertFromString("#EFEFF2");
        public Color ArrowBrushComboBox = (Color)ColorConverter.ConvertFromString("#ddd");
        public Color HoverToggleBackgroundComboBox = (Color)ColorConverter.ConvertFromString("#979797");
        public Color HoverItemComboBox = (Color)ColorConverter.ConvertFromString("#979797");
        public Color EnableButtonOn = (Color)ColorConverter.ConvertFromString("#979797");
        public Color EnableButtonOff = (Color)ColorConverter.ConvertFromString("#979797");


        public static void ApplyVisuals(VisualsSettings cs)//Применить переданный набор настроек интерфейса
        {
            if (cs == null)
                throw new ArgumentNullException();
            App.Current.Resources["WindowBorderThickness"] = cs.WindowBorderThickness;

            foreach (var fi in typeof(VisualsSettings).GetRuntimeFields())
                if (fi.FieldType == typeof(Color))
                    App.Current.Resources[fi.Name] = new SolidColorBrush((Color)fi.GetValue(cs));
        }
    }
}
