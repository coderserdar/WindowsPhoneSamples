// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainView.xaml.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   Defines the Main View.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Navigation;
using AuthenticationSample.WP80.Helpers;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight.Ioc;

namespace AuthenticationSample.WP80.Views
{
    /// <summary>
    /// Defines the Main View.
    /// </summary>
    public partial class MainView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainView"/> class.
        /// </summary>
        public MainView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
            navigationService.RemoveAllBackStack();
            base.OnNavigatedTo(e);
        }
    }
}