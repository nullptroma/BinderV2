using BindModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.WpfControls.BindControl
{
    interface IBindElement
    {
        Bind GetBind();
        bool Selected { get; set; }
    }
}
