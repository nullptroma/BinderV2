﻿#pragma checksum "..\..\..\..\Views\MainWindow\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "66294343DD5B7DB9A603767418C39851AC7AD36DC3894FD474D6ECB24EA13F4E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using BinderV2.Windows.Main;
using Hardcodet.Wpf.TaskbarNotification;
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
using Trigger.Types;


namespace BinderV2.Windows.Main {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Hardcodet.Wpf.TaskbarNotification.TaskbarIcon TaskBarIcon;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ShowWindowButton;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem HideWindowButton;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ExitBut;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer BindsScrollViewer;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ScriptBox;
        
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
            System.Uri resourceLocater = new System.Uri("/BinderV2;component/views/mainwindow/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
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
            
            #line 12 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
            ((BinderV2.Windows.Main.MainWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
            ((BinderV2.Windows.Main.MainWindow)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TaskBarIcon = ((Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)(target));
            
            #line 27 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
            this.TaskBarIcon.TrayMouseDoubleClick += new System.Windows.RoutedEventHandler(this.TaskBarIcon_TrayMouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ShowWindowButton = ((System.Windows.Controls.MenuItem)(target));
            
            #line 30 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
            this.ShowWindowButton.Click += new System.Windows.RoutedEventHandler(this.ShowWindowButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.HideWindowButton = ((System.Windows.Controls.MenuItem)(target));
            
            #line 31 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
            this.HideWindowButton.Click += new System.Windows.RoutedEventHandler(this.HideWindowButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ExitBut = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 6:
            
            #line 48 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BindsScrollViewer = ((System.Windows.Controls.ScrollViewer)(target));
            
            #line 61 "..\..\..\..\Views\MainWindow\MainWindow.xaml"
            this.BindsScrollViewer.ScrollChanged += new System.Windows.Controls.ScrollChangedEventHandler(this.BindsScrollViewer_ScrollChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ScriptBox = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

