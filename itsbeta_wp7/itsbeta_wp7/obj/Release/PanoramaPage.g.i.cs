﻿#pragma checksum "C:\Users\m0rg0_000\Documents\GitHub\achievements-mobile\itsbeta_wp7\itsbeta_wp7\PanoramaPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4A17D0D1CD0FA538D77892C06DDA5E3B"
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
using Telerik.Windows.Controls;
using WPExtensions;


namespace itsbeta_wp7 {
    
    
    public partial class PanoramaPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal WPExtensions.AdvancedApplicationBarMenuItem about_program;
        
        internal Telerik.Windows.Controls.RadJumpList AllCategories;
        
        internal System.Windows.Controls.Button QrReaderButton;
        
        internal System.Windows.Controls.Button ActivateCOdeButton;
        
        internal System.Windows.Controls.Grid Bonuses;
        
        internal System.Windows.Controls.Grid Subcategories;
        
        internal Telerik.Windows.Controls.RadJumpList LastAchieves;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/itsbeta_wp7;component/PanoramaPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.about_program = ((WPExtensions.AdvancedApplicationBarMenuItem)(this.FindName("about_program")));
            this.AllCategories = ((Telerik.Windows.Controls.RadJumpList)(this.FindName("AllCategories")));
            this.QrReaderButton = ((System.Windows.Controls.Button)(this.FindName("QrReaderButton")));
            this.ActivateCOdeButton = ((System.Windows.Controls.Button)(this.FindName("ActivateCOdeButton")));
            this.Bonuses = ((System.Windows.Controls.Grid)(this.FindName("Bonuses")));
            this.Subcategories = ((System.Windows.Controls.Grid)(this.FindName("Subcategories")));
            this.LastAchieves = ((Telerik.Windows.Controls.RadJumpList)(this.FindName("LastAchieves")));
        }
    }
}

