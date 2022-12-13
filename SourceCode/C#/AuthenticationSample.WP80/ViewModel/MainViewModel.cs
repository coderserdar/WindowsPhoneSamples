// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   The main view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows.Input;
using AuthenticationSample.WP80.Resources;
using AuthenticationSample.WP80.Services.Interfaces;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AuthenticationSample.WP80.ViewModel
{
    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILogManager _logManager;
        private readonly ISessionService _sessionService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// <param name="navigationService">
        /// The navigation Service.
        /// </param>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        /// <param name="sessionService">The session service.</param>
        public MainViewModel(INavigationService navigationService, 
            ILogManager logManager, 
            ISessionService sessionService)
        {
            _navigationService = navigationService;
            _logManager = logManager;
            _sessionService = sessionService;
            LogoutCommand = new RelayCommand(
                () =>
                {
                    _sessionService.Logout();
                    _navigationService.NavigateTo(new Uri(Constants.LoginView, UriKind.Relative));
                });
            AboutCommand = new RelayCommand(
                async () =>
                { 
                    Exception exception = null;
                    try
                    {
                        _navigationService.NavigateTo(new Uri(Constants.AboutView, UriKind.Relative));
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    if (exception != null)
                    {
                        await _logManager.LogAsync(exception);
                    }
                });
        }
        
        /// <summary>
        /// Gets the about command.
        /// </summary>
        /// <value>
        /// The about command.
        /// </value>
        public ICommand AboutCommand { get; private set; }

        /// <summary>
        /// Gets the logout command.
        /// </summary>
        /// <value>
        /// The logout command.
        /// </value>
        public ICommand LogoutCommand { get; private set; }
    }
}