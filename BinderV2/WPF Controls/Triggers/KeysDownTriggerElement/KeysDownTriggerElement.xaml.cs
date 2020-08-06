using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Hooks.Keyboard;
using Trigger.Types;

namespace BinderV2.WpfControls.Triggers
{
    /// <summary>
    /// Логика взаимодействия для KeysDownTriggerElement.xaml
    /// </summary>
    public partial class KeysDownTriggerControl : UserControl, ITriggerControl
    {
        public KeysDownTrigger trigger;
        

        public void UpdateVisualization()//обновляет все цвета и текст на кнопке
        {
            if (selected)
            {
                border.SetResourceReference(Border.BackgroundProperty, "BindTriggerBackgroundSelected");
                border.SetResourceReference(Border.BorderBrushProperty, "BindTriggerBorderSelected");
            }
            else
            {
                border.SetResourceReference(Border.BackgroundProperty, "BindTriggerBackgroundUnSelected");
                border.SetResourceReference(Border.BorderBrushProperty, "BindTriggerBorderUnSelected");
            }
            if (trigger.EnableTrigger)
            {
                EnableButton.SetResourceReference(Button.BackgroundProperty, "EnableButtonOn");
                EnableButton.Content = "Активен";
            }
            else
            {
                EnableButton.SetResourceReference(Button.BackgroundProperty, "EnableButtonOff");
                EnableButton.Content = "Неактивен";
            }
        }

        public KeysDownTriggerControl(KeysDownTrigger trigger)
        {
            InitializeComponent();
            this.trigger = trigger;
            this.trigger.EnableChanged += (sender, e) => { UpdateVisualization(); };
            labelName.Content = this.trigger.Name; 
            labelKeys.Content = trigger.Keys.Count > 0 ? string.Join(" + ", this.trigger.Keys) : "<клавиши>";
            UpdateVisualization();
        }


        private void labelKeys_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BaseTrigger.EnableAllTriggers = false;//отключаем все триггеры, а соотвественно бинды, чтобы записать новые кнопки.
            trigger.Keys.Clear();

            KeyEventHandlerCustom keyDown = null;
            keyDown = (s, keyArgs) => 
            {
                trigger.Keys.Add(keyArgs.Key);
                labelKeys.Content = string.Join(" + ", trigger.Keys);
            };

            KeyEventHandlerCustom keyUp = null;
            keyUp = (s, keyArgs) => 
            {
                KeyboardHook.KeyDown -= keyDown;
                KeyboardHook.KeyUp -= keyUp;
                BaseTrigger.EnableAllTriggers = true;//когда записали, включаем обратно
            };

            KeyboardHook.KeyDown += keyDown;
            KeyboardHook.KeyUp += keyUp;

            labelKeys.Content = "<нажмите сочетание>";
        }

        private void EditNameButton_Click(object sender, RoutedEventArgs e)
        {
            StartEditName();
        }

        private void StartEditName()
        {
            labelName.Visibility = Visibility.Collapsed;
            textBoxName.Visibility = Visibility.Visible;
            textBoxName.SelectionStart = textBoxName.Text.Length;
            textBoxName.Focus();
        }


        private void textBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EndEditName();
            }
        }

        private void EndEditName()
        {
            trigger.Name = textBoxName.Text;
            labelName.Content = trigger.Name;
            labelName.Visibility = Visibility.Visible;
            textBoxName.Visibility = Visibility.Collapsed;
            labelName.Focus();
        }


        #region ITriggerElement
        private bool selected = false;

        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                UpdateVisualization();
            }
        }

        public BaseTrigger GetTrigger()
        {
            return trigger;
        }
        #endregion

        private void EnableButton_Click(object sender, RoutedEventArgs e)
        {
            trigger.EnableTrigger = !trigger.EnableTrigger;
        }
    }
}
