using BinderV2.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using BinderV2.Windows.TriggersEdit;
using BindModel;

namespace BinderV2.WpfControls.BindControl
{
    /// <summary>
    /// Логика взаимодействия для BindElement.xaml
    /// </summary>
    public partial class BindElement : UserControl, IBindElement
    {
        private Bind bind;
        

        public BindElement(Bind b)
        {
            InitializeComponent();
            bind = b;
            labelName.Content = bind.Name;
            Idlabel.Content = "Id=" + bind.Id;
            bind.EnableChanged += (sender, e) => 
            {
                UpdateVisualization();
            };
            UpdateVisualization();
        }

        public BindElement() : this(new Bind())
        {
            
        }

        public void UpdateVisualization()//обновляет все цвета и текст на кнопке
        {
            this.Dispatcher.Invoke(()=> 
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
                if (bind.Enable)
                {
                    EnableButton.SetResourceReference(Button.BackgroundProperty, "EnableButtonOn");
                    EnableButton.Content = "Активен";
                }
                else
                {
                    EnableButton.SetResourceReference(Button.BackgroundProperty, "EnableButtonOff");
                    EnableButton.Content = "Неактивен";
                }
            });
        }


        private void StartEditName()
        {
            labelName.Visibility = Visibility.Collapsed;
            textBoxName.Visibility = Visibility.Visible;
            textBoxName.SelectionStart = textBoxName.Text.Length;
            textBoxName.Focus();
        }

        private void EndEditName()
        {
            bind.Name = textBoxName.Text;
            labelName.Content = bind.Name;
            labelName.Visibility = Visibility.Visible;
            textBoxName.Visibility = Visibility.Collapsed;
            labelName.Focus();
        }

        private void EditNameButton_Click(object sender, RoutedEventArgs e)
        {
            StartEditName();
        }


        private void textBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EndEditName();
            }
        }
        

        private void TriggerEditButton_Click(object sender, RoutedEventArgs e)
        {
            new TriggersEditWindow(bind).ShowDialog();
        }

        private void EnableButton_Click(object sender, RoutedEventArgs e)
        {
            bind.Enable = !bind.Enable;
        }

        #region IBindElement
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

        public Bind GetBind()
        {
            return bind;
        }
        #endregion
    }
}
