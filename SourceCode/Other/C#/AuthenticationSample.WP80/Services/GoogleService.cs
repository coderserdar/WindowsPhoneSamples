// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleService.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   The google service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using AuthenticationSample.WP80.Resources;
using AuthenticationSample.WP80.Services.Interfaces;
using AuthenticationSample.WP80.Services.Model;
using Cimbalino.Phone.Toolkit.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;
using Microsoft.Phone.Controls;

namespace AuthenticationSample.WP80.Services
{
    /// <summary>
    /// The google service.
    /// </summary>
    public class GoogleService : IGoogleService
    {
        private readonly ILogManager _logManager;
        private readonly IStorageService _storageService;
        private UserCredential _credential;
        private Oauth2Service _authService;
        private Userinfoplus _userinfoplus;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleService" /> class.
        /// </summary>
        /// <param name="logManager">The log manager.</param>
        /// <param name="storageService">The storage service.</param>
        public GoogleService(ILogManager logManager, IStorageService storageService)
        {
            _logManager = logManager;
            _storageService = storageService;
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
                // Oauth2Service.Scope.UserinfoEmail
                _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
                {
                    ClientId = Constants.GoogleClientId,
                    ClientSecret = Constants.GoogleClientSecret
                }, new[] { Oauth2Service.Scope.UserinfoProfile }, "user", CancellationToken.None);
                
                var session = new Session
                {
                    AccessToken = _credential.Token.AccessToken,
                    Provider = Constants.GoogleProvider,
                    ExpireDate =
                        _credential.Token.ExpiresInSeconds != null
                            ? new DateTime(_credential.Token.ExpiresInSeconds.Value)
                            : DateTime.Now.AddYears(1),
                    Id = string.Empty
                };

                return session;
            }
            catch (TaskCanceledException taskCanceledException)
            {
                throw new InvalidOperationException("Login canceled.", taskCanceledException);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            await _logManager.LogAsync(exception);
            return null;
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <returns>
        /// The user info.
        /// </returns>
        public async Task<Userinfoplus> GetUserInfo()
        {
            _authService = new Oauth2Service(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = AppResources.ApplicationTitle,
            });
            _userinfoplus = await _authService.Userinfo.V2.Me.Get().ExecuteAsync();

            return _userinfoplus;
        }

        /// <summary>
        /// The logout.
        /// </summary>
        public async void Logout()
        {
            await new WebBrowser().ClearCookiesAsync();
            if (_storageService.FileExists(Constants.GoogleTokenFileName))
            {
                _storageService.DeleteFile(Constants.GoogleTokenFileName);
            }
        }
    }
}
