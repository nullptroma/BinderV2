using BinderV2.Settings.Visuals.Events;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace BinderV2.Settings.Visuals
{
    public class VisualsSettings
    {
        public Thickness WindowBorderThickness = new Thickness(2);
        public Thickness WindowIconMargin = new Thickness(1,1,1,1);
        public double HeightWindowTitle = 32;
        public double WindowIconSize = 30;
        public double TitleFontSize = 12;
        public Color WindowBackground = (Color)ColorConverter.ConvertFromString("#EFEFF2");
        public Color WindowBorderBrush = (Color)ColorConverter.ConvertFromString("#747474");
        public Color WindowBorderBrushInactive = (Color)ColorConverter.ConvertFromString("#999999");
        public Color WindowStatusForeground = (Color)ColorConverter.ConvertFromString("#FFFFFF");
        public Color WindowStatusForegroundInactive = (Color)ColorConverter.ConvertFromString("#FFFFFF");
        public Color WindowButtonsColor = (Color)ColorConverter.ConvertFromString("#FF000000");
        public Color WindowButtonsColorInactive = (Color)ColorConverter.ConvertFromString("#FF000000");
        public Color WindowButtonsMouseOverColor = (Color)ColorConverter.ConvertFromString("#FFF");
        public Color TitleBarBackgroundBrush = (Color)ColorConverter.ConvertFromString("#EFEFF2");
        public Color TitleBarForegroundBrush = (Color)ColorConverter.ConvertFromString("#FF000000");
        public Color BorderColors = (Color)ColorConverter.ConvertFromString("#FFB4B4B4");
        public Color TextBoxColor = (Color)ColorConverter.ConvertFromString("#FFEFEFF2");
        public Color TextBoxBorderColor = (Color)ColorConverter.ConvertFromString("#B1787878");
        public Color TextBoxBorderMouseOverColor = (Color)ColorConverter.ConvertFromString("#FF78AEF8");
        public Color TextColor = (Color)ColorConverter.ConvertFromString("#FF000000");
        public Color TextBoxSelectionColor = (Color)ColorConverter.ConvertFromString("#FF0078D7");
        public Color BindTriggerBackgroundUnSelected = (Color)ColorConverter.ConvertFromString("#FFB9B9B9");
        public Color BindTriggerBorderUnSelected = (Color)ColorConverter.ConvertFromString("#FF616161");
        public Color BindTriggerBorderSelected = (Color)ColorConverter.ConvertFromString("#FF328232");
        public Color BindTriggerBackgroundSelected = (Color)ColorConverter.ConvertFromString("#FFB9C3B9");
        public Color ScrollViewerColor = (Color)ColorConverter.ConvertFromString("#51000000");
        public Color ComboboxBackground = (Color)ColorConverter.ConvertFromString("#EFEFF2");
        public Color BackgroundBrushPopUpComboBox = (Color)ColorConverter.ConvertFromString("#EFEFF2");
        public Color ArrowBrushComboBox = (Color)ColorConverter.ConvertFromString("#ddd");
        public Color HoverToggleBackgroundComboBox = (Color)ColorConverter.ConvertFromString("#979797");
        public Color HoverItemComboBox = (Color)ColorConverter.ConvertFromString("#979797");
        public Color EnableButtonOn = (Color)ColorConverter.ConvertFromString("#979797");
        public Color EnableButtonOff = (Color)ColorConverter.ConvertFromString("#979797");
        public Color ButtonsBackground = (Color)ColorConverter.ConvertFromString("#FFDDDDDD");
        public Color ButtonBackgroundInactive = (Color)ColorConverter.ConvertFromString("#55010101");
        public Color ButtonBorderColor = (Color)ColorConverter.ConvertFromString("#000");
        public Color ButtonBackgroundMouseOver = (Color)ColorConverter.ConvertFromString("#55010101");
        public Color MenuItemColor = (Color)ColorConverter.ConvertFromString("Transparent");
        public Color MenuPopupBrush = (Color)ColorConverter.ConvertFromString("#FFEFEFF2");

        public static event VisualizeSettingsChangedEventHandler VisualizeSettingsChanged;
        public static void ApplyVisuals(VisualsSettings cs)//Применить переданный набор настроек интерфейса
        {
            if (cs == null)
                throw new ArgumentNullException();
            App.Current.Resources["WindowBorderThickness"] = cs.WindowBorderThickness;
            App.Current.Resources["WindowIconMargin"] = cs.WindowIconMargin;
            App.Current.Resources["HeightWindowTitle"] = cs.HeightWindowTitle;
            App.Current.Resources["WindowIconSize"] = cs.WindowIconSize;
            App.Current.Resources["TitleFontSize"] = cs.TitleFontSize;
            App.Current.Resources["CaptionWindowHeight"] = cs.HeightWindowTitle + cs.WindowBorderThickness.Top;

            foreach (var fi in typeof(VisualsSettings).GetRuntimeFields())
                if (fi.FieldType == typeof(Color))
                    App.Current.Resources[fi.Name] = new SolidColorBrush((Color)fi.GetValue(cs));

            VisualizeSettingsChanged?.Invoke(null, new VisualizeSettingsChangedEventArgs(cs));
        }
    }
}
