﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E3B6A87978858BF0FDAD3C1C534781DE9DCB8F4B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Calendar;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace Calendar {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/LunchPlanner;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 4:
            
            #line 431 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.n);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 436 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Calendar)(target)).KeyUp += new System.Windows.Input.KeyEventHandler(this.Calendar_KeyUp);
            
            #line default
            #line hidden
            
            #line 436 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Calendar)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.Calendar_MouseDoubleClick);
            
            #line default
            #line hidden
            
            #line 436 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Calendar)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Calendar_Loaded);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 437 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 439 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.ListView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ListView_Loaded);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 331 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 331 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.Grid_MouseEnter);
            
            #line default
            #line hidden
            
            #line 331 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.Grid_MouseLeave);
            
            #line default
            #line hidden
            break;
            case 2:
            
            #line 361 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.Grid_MouseEnter);
            
            #line default
            #line hidden
            
            #line 361 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.Grid_MouseLeave);
            
            #line default
            #line hidden
            break;
            case 3:
            
            #line 363 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.PART_MonthView_Loaded);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

