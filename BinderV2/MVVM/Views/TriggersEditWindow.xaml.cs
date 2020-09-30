using Trigger.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BindModel;
using BinderV2.MVVM.ViewModels;

namespace BinderV2.MVVM.Views
{
    public partial class TriggersEditWindow : Window
    {
        public TriggersEditWindow(Bind bind)
        {
            InitializeComponent();
            DataContext = new TriggerEditViewModel(bind);//В TriggerEditViewModel мне нужны и bind, и его закрытое поле triggers
            Title = "Редактирование " + bind.Name;
        }
       

        private async void TriggerScrollViewer_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            if(e.ExtentHeightChange!=0)
                await ScrollDown(TriggerScrollViewer).ConfigureAwait(true);
        }

        private async Task ScrollDown(System.Windows.Controls.ScrollViewer sv)
        {
            for (var i = sv.ContentVerticalOffset; i < sv.ScrollableHeight; i += ((sv.ScrollableHeight - i) / 10) + 1)
            {
                sv.ScrollToVerticalOffset(i);
                await Task.Delay(1).ConfigureAwait(true);
            }
        }
    }
}
