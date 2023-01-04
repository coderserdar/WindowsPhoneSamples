// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   The about view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows.Input;
using AuthenticationSample.WP80.Resources;
using AuthenticationSample.WP80.Services.Interfaces;
using Cimbalino.Phone.Toolkit.Helpers;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AuthenticationSample.WP80.ViewModel
{
    /// <summary>
    /// The about view model.
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        /// <summary>
        /// The email compose service.
        /// </summary>
        private readonly IEmailComposeService _emailComposeService;
        
        /// <summary>
        /// The marketplace review service.
        /// </summary>
        private readonly IMarketplaceReviewService _marketplaceReviewService;

        /// <summary>
        /// The share link service.
        /// </summary>
        private readonly IShareLinkService _shareLinkService;

        /// <summary>
        /// The log manager.
        /// </summary>
        private readonly ILogManager _logManager;

        /// <summary>
        /// The public application url.
        /// </summary>
        private readonly string _appUrl;

        /// <summary>
        /// The application manifest.
        /// </summary>
        private readonly ApplicationManifest _applicationManifest;

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel"/> class.
        /// </summary>
        /// <param name="emailComposeService">
        /// The email Compose Service.
        /// </param>
        /// <param name="applicationManifestService">
        /// The application Manifest Service.
        /// </param>
        /// <param name="marketplaceReviewService">
        /// The marketplace review service.
        /// </param>
        /// <param name="shareLinkService">
        /// The share Link Service.
        /// </param>
        /// <param name="logManager">The log manager.</param>
        public AboutViewModel(
            IEmailComposeService emailComposeService,
            IApplicationManifestService applicationManifestService,
            IMarketplaceReviewService marketplaceReviewService,
            IShareLinkService shareLinkService,
            ILogManager logManager)
        {
            _emailComposeService = emailComposeService;
            _marketplaceReviewService = marketplaceReviewService;
            _shareLinkService = shareLinkService;
            _logManager = logManager;
            RateCommand = new RelayCommand(Rate);
            SendFeedbackCommand = new RelayCommand(SendFeedback);
            ShareToMailCommand = new RelayCommand(ShareToMail);
            ShareSocialNetworkCommand = new RelayCommand(ShareSocialNetwork);
            _applicationManifest = applicationManifestService.GetApplicationManifest();
            _appUrl = string.Concat("http://windowsphone.com/s?appid=", _applicationManifest.App.ProductId.Replace("{", string.Empty)).Replace("}", string.Empty);
        }

        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string Author
        {
            get
            {
                return _applicationManifest.App.Author;
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version
        {
            get
            {
                return _applicationManifest.App.Version;
            }
        }

        /// <summary>
        /// Gets the rate command.
        /// </summary>
        /// <value>
        /// The rate command.
        /// </value>
        public ICommand RateCommand { get; private set; }

        /// <summary>
        /// Gets the send feedback command.
        /// </summary>
        /// <value>
        /// The send feedback command.
        /// </value>
        public ICommand SendFeedbackCommand { get; private set; }

        /// <summary>
        /// Gets the share social network command.
        /// </summary>
        /// <value>
        /// The share social network command.
        /// </value>
        public ICommand ShareSocialNetworkCommand { get; private set; }

        /// <summary>
        /// Gets the share to e-mail command.
        /// </summary>
        /// <value>
        /// The share to mail command.
        /// </value>
        public ICommand ShareToMailCommand { get; private set; }

        /// <summary>
        /// The rate action.
        /// </summary>
        private async void Rate()
        {
            Exception exception = null;
            try
            {
                _marketplaceReviewService.Show();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception != null)
            {
                await _logManager.LogAsync(exception);
            }
        }

        /// <summary>
        /// The send feedback.
        /// </summary>
        private async void SendFeedback()
        {
            Exception exception = null;
            try
            {
                var subject = string.Format(
                               AppResources.AboutView_Feedback,
                               _applicationManifest.App.Title,
                               _applicationManifest.App.Version);
                _emailComposeService.Show(AppResources.AboutView_EmailForFeedback, subject, string.Empty);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception != null)
            {
                await _logManager.LogAsync(exception);
            }
        }

        /// <summary>
        /// The share social network.
        /// </summary>
        private async void ShareSocialNetwork()
        {
            Exception exception = null;
            try
            {
                _shareLinkService.Show(_applicationManifest.App.Title, AppResources.AboutView_ShareSocialNetwork, new Uri(_appUrl, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (exception != null)
            {
                await _logManager.LogAsync(exception);
            }
        }

        /// <summary>
        /// The share to mail.
        /// </summary>
        private async void ShareToMail()
        {
            Exception exception = null;
            try
            {
                var body = string.Format(AppResources.AboutView_ShareEmailBody, _appUrl);
                _emailComposeService.Show(AppResources.AboutView_ShareEmailSubject, body);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception != null)
            {
                await _logManager.LogAsync(exception);
            }
        }
    }
}
