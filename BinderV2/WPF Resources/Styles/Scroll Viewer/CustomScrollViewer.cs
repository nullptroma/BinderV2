using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomScrollViewerLogic
{
    partial class CustomScrollViewer
    {
        private async Task ScrollDown(ScrollViewer sv)
        {
            for (var i = sv.ContentVerticalOffset; i < sv.ScrollableHeight; i += ((sv.ScrollableHeight - i) / 10) + 1)
            {
                sv.ScrollToVerticalOffset(i);
                await Task.Delay(1);
            }
        }

        private async void CustomScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0)
                await ScrollDown((ScrollViewer)sender);
        }

        private async void CustomScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            int length = (int)(e.Delta * -1);
            double step = length / 12;
            for (int i = 0; i < 12; i++)
            {
                ((ScrollViewer)sender).Dispatcher.Invoke(() => ((ScrollViewer)sender).ScrollToVerticalOffset(((ScrollViewer)sender).VerticalOffset + step));
                await Task.Delay(1);
            }
        }
    }
}
