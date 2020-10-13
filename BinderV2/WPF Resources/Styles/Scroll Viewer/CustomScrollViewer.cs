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
    public partial class CustomScrollViewer
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
            var tag = ((ScrollViewer)sender).Tag;
            if (tag != null && (bool)tag)
                if (e.ExtentHeightChange > 60)
                    await ScrollDown((ScrollViewer)sender);
        }

        private async void CustomScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            int length = (int)(e.Delta * -1);
            double step = length / 10;
            for (int i = 0; i < 20; i++)
            {
                step /= 1.04;
                var verticalOffset = ((ScrollViewer)sender).VerticalOffset + step;
                ((ScrollViewer)sender).Dispatcher.Invoke(() => ((ScrollViewer)sender).ScrollToVerticalOffset(verticalOffset));
                await Task.Delay(1);

                if (step < 0)
                    if (verticalOffset < ((ScrollViewer)sender).VerticalOffset)
                        return;
                if (step > 0)
                    if (verticalOffset > ((ScrollViewer)sender).VerticalOffset)
                        return;
            }
        }
    }
}
