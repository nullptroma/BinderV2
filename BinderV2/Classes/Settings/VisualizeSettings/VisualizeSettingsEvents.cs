using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.Settings.Visuals.Events
{
    public class VisualizeSettingsChangedEventArgs : EventArgs
    {
        public VisualsSettings newVS;
        public VisualizeSettingsChangedEventArgs(VisualsSettings vs)
        {
            newVS = vs;
        }
    }

    public delegate void VisualizeSettingsChangedEventHandler(object sender, VisualizeSettingsChangedEventArgs e);
}
