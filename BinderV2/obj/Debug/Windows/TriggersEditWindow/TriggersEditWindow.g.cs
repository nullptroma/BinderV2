﻿#pragma checksum "..\..\..\..\Windows\TriggersEditWindow\TriggersEditWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "98370F7CA2656C7CB9C72F68FA3FA58DFE4D905753F12E77839C1D1C4C8684CB"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using BinderV2.Windows.TriggersEdit;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using Microsoft.Xaml.Behaviors.Input;
using Microsoft.Xaml.Behaviors.Layout;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace BinderV2.Windows.TriggersEdit {
    
    
    /// <summary>
    /// TriggersEditWindow
    /// </summary>
    public partial class TriggersEditWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 36 "..\..\..\..\Windows\TriggersEditWindow\TriggersEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer TriggerScrollViewer;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\Windows\TriggersEditWindow\TriggersEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox scriptTextBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/BinderV2;component/windows/triggerseditwindow/triggerseditwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\TriggersEditWindow\TriggersEditWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.TriggerScrollViewer = ((System.Windows.Controls.ScrollViewer)(target));
            
            #line 36 "..\..\..\..\Windows\TriggersEditWindow\TriggersEditWindow.xaml"
            this.TriggerScrollViewer.ScrollChanged += new System.Windows.Controls.ScrollChangedEventHandler(this.TriggerScrollViewer_ScrollChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.scriptTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 67 "..\..\..\..\Windows\TriggersEditWindow\TriggersEditWindow.xaml"
            this.scriptTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.scriptTextBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 68 "..\..\..\..\Windows\TriggersEditWindow\TriggersEditWindow.xaml"
            this.scriptTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.scriptTextBox_LostFocus);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

