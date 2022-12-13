// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginViewModel.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   The login view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Input;
using AuthenticationSample.WP80.Resources;
using AuthenticationSample.WP80.Services.Interfaces;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AuthenticationSample.WP80.ViewModel
{
    /// <summary>
    /// The login view model.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILogManager _logManager;
        private readonly IMessageBoxService _messageBox;
        private readonly INavigationService _navigationService;
        private readonly ISessionService _sessionService;
        private bool _inProgress;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">
        /// The navigation service.
        /// </param>
        /// <param name="sessionService">
        /// The session service.
        /// </param>
        /// <param name="messageBox">
        /// The message box.
        /// </param>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        public LoginViewModel(INavigationService navigationService,
            ISessionService sessionService,
            IMessageBoxService messageBox,
            ILogManager logManager)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _messageBox = messageBox;
            _logManager = logManager;
            LoginCommand = new RelayCommand<string>(LoginAction);
        }

        /// <summary>
        /// Gets or sets a value indicating whether in progress.
        /// </summary>
        /// <value>
        /// The in progress.
        /// </value>
        public bool InProgress
        {
            get { return _inProgress; }
            set { Set(() => InProgress, ref _inProgress, value); }
        }

        /// <summary>
        /// Gets the facebook login command.
        /// </summary>
        /// <value>
        /// The facebook login command.
        /// </value>
        public ICommand LoginCommand { get; private set; }

        /// <summary>
        /// Facebook's login action.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        private async void LoginAction(string provider)
        {
            Exception exception = null;
            bool isToShowMessage = false;
            try
            {
                InProgress = true;
                var auth = await _sessionService.LoginAsync(provider);
                if (!auth)
                {
                    await _messageBox.ShowAsync(AppResources.LoginView_LoginNotAllowed_Message,
                        AppResources.MessageBox_Title,
                        new List<string>
                    {
                        AppResources.Button_OK
                    });
                }
                else
                {
                    _navigationService.NavigateTo(new Uri(Constants.MainView, UriKind.Relative));
                }

                InProgress = false;
            }
            catch (InvalidOperationException e)
            {
                InProgress = false;
                isToShowMessage = true;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (isToShowMessage)
            {
                await _messageBox.ShowAsync(AppResources.LoginView_AuthFail, AppResources.ApplicationTitle, new List<string> { AppResources.Button_OK });
            }
            if (exception != null)
            {
                await _logManager.LogAsync(exception);
            }
        }
    }
}
