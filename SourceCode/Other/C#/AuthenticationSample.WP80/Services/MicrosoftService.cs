// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftService.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   Defines the MicrosoftService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationSample.WP80.Resources;
using AuthenticationSample.WP80.Services.Interfaces;
using AuthenticationSample.WP80.Services.Model;
using Microsoft.Live;

namespace AuthenticationSample.WP80.Services
{
    /// <summary>
    /// The microsoft service.
    /// </summary>
    public class MicrosoftService : IMicrosoftService
    {
        private readonly ILogManager _logManager;
        private LiveAuthClient _authClient;
        private LiveConnectSession _liveSession;

        /// <summary>
        /// Defines the scopes the application needs.
        /// </summary>
        private static readonly string[] Scopes = { "wl.signin", "wl.basic", "wl.offline_access" };
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftService"/> class.
        /// </summary>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        public MicrosoftService(ILogManager logManager)
        {
            _logManager = logManager;
        }

        /// <summary>
        /// The login async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        public async Task<Session> LoginAsync()
        {
            Exception exception = null;
            try
            {
                _authClient = new LiveAuthClient(Constants.MicrosoftClientId);
                var loginResult = await _authClient.InitializeAsync(Scopes);
                var result = await _authClient.LoginAsync(Scopes);
                if (result.Status == LiveConnectSessionStatus.Connected)
                {
                    _liveSession = loginResult.Session;
                    var session = new Session
                    {
                        AccessToken = result.Session.AccessToken,
                        ExpireDate = result.Session.Expires.DateTime,
                        Provider = Constants.MicrosoftProvider,
                    };

                    return session;
                }
            }
            catch (LiveAuthException ex)
            {
                throw new InvalidOperationException("Login canceled.", ex);
            }
            catch (Exception e)
            {
                exception = e;
            }

            await _logManager.LogAsync(exception);
            return null;
        }

        /// <summary>
        /// The logout.
        /// </summary>
        public async void Logout()
        {
            if (_authClient == null)
            {
                _authClient = new LiveAuthClient(Constants.MicrosoftClientId);
                var loginResult = await _authClient.InitializeAsync(Scopes);
            }

            _authClient.Logout();
        }
    }
}
