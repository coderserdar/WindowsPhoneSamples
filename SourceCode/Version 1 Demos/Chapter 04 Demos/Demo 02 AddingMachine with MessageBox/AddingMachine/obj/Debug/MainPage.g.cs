﻿#pragma checksum "C:\Users\rob\Desktop\AddingMachine\AddingMachine\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F6BD394F0BCAD7BF2C88BE6397E9F731"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace AddingMachine {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox firstNumberTextBox;
        
        internal System.Windows.Controls.TextBox secondNumberTextBox;
        
        internal System.Windows.Controls.Button equalsButton;
        
        internal System.Windows.Controls.TextBlock resultTextBlock;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/AddingMachine;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.firstNumberTextBox = ((System.Windows.Controls.TextBox)(this.FindName("firstNumberTextBox")));
            this.secondNumberTextBox = ((System.Windows.Controls.TextBox)(this.FindName("secondNumberTextBox")));
            this.equalsButton = ((System.Windows.Controls.Button)(this.FindName("equalsButton")));
            this.resultTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("resultTextBlock")));
        }
    }
}

