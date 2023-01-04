// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginView.xaml.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   Defines the LoginView.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Navigation;
using AuthenticationSample.WP80.Helpers;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight.Ioc;

namespace AuthenticationSample.WP80.Views
{
    /// <summary>
    /// Defines the LoginView.
    /// </summary>
    public partial class LoginView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginView"/> class.
        /// </summary>
        public LoginView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The on navigated to.
        /// </summary>
        /// <param name="e">
        /// The e parameter.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
            navigationService.RemoveAllBackStack();
            base.OnNavigatedTo(e);
        }
    }
}