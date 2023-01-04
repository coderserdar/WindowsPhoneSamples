// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FacebookService.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   Defines the Facebook Service
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using AuthenticationSample.WP80.Resources;
using AuthenticationSample.WP80.Services.Interfaces;
using AuthenticationSample.WP80.Services.Model;
using Facebook.Client;
using Microsoft.Phone.Controls;

namespace AuthenticationSample.WP80.Services
{
    /// <summary>
    /// Defines the Facebook Service.
    /// </summary>
    public class FacebookService : IFacebookService
    {
        private readonly ILogManager _logManager;
        private readonly FacebookSessionClient _facebookSessionClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookService"/> class.
        /// </summary>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        public FacebookService(ILogManager logManager)
        {
            _logManager = logManager;
            _facebookSessionClient = new FacebookSessionClient(Constants.FacebookAppId);
        }

        /// <summary>
        /// The login sync.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        public async Task<Session> LoginAsync()
        {
            Exception exception;
            Session sessionToReturn = null;
            try
            {
                var session = await _facebookSessionClient.LoginAsync("user_about_me,read_stream");
                sessionToReturn = new Session
                {
                    AccessToken = session.AccessToken,
                    Id = session.FacebookId,
                    ExpireDate = session.Expires,
                    Provider = Constants.FacebookProvider
                };

                return sessionToReturn;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            await _logManager.LogAsync(exception);
            return sessionToReturn;
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        public async void Logout()
        {
            Exception exception = null;
            try
            {
                _facebookSessionClient.Logout();

                // clean all cookies from browser, is a workarround
                await new WebBrowser().ClearCookiesAsync();
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
