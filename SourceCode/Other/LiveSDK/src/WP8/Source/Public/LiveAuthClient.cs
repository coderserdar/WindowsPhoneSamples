// ------------------------------------------------------------------------------
// Copyright 2014 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ------------------------------------------------------------------------------

namespace Microsoft.Live
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    using Microsoft.Live.Operations;

    /// <summary>
    /// This is the class that applications use to authenticate the user and obtain access token after user
    /// grants consent to the application.
    /// </summary>
    public sealed partial class LiveAuthClient : INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Creates a new instance of the auth client.
        /// <param name="clientId">Client id of the application.</param>
        /// </summary>
        public LiveAuthClient(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException(
                    string.Format(ResourceHelper.GetString("InvalidNullParameter"), "clientId"), "clientId");
            }

            this.AuthClient = new PhoneAuthClient(this);
            this.InitializeMembers(clientId, null);
        }

        #endregion

        #region Properties
        
        private ThemeType Theme
        {
            get { return Platform.GetThemeType(); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the auth client. Detects if user is already signed in,
        /// If user is already signed in, creates a valid Session.
        /// This call is UI-less.
        /// </summary>
        /// <returns>A Task object representing the asynchronous operation.</returns>
        public Task<LiveLoginResult> InitializeAsync()
        {
            return this.InitializeAsync(null);
        }

        /// <summary>
        /// Initializes the auth client. Detects if user is already signed in,
        /// If user is already signed in, creates a valid Session.
        /// This call is UI-less.
        /// </summary>
        /// <param name="scopes">The list of offers that the application is requesting user consent for.</param>
        /// <returns>A Task object representing the asynchronous operation.</returns>
        public async Task<LiveLoginResult> InitializeAsync(IEnumerable<string> scopes)
        {
            this.PrepareForAsync();

            // Always make a call to the server instead of relying on cached access token for two reasons:
            // 1. user may have revoked the scope
            // 2. user may have previously consented to the scope via a web app, we should not ask again.

            // Use a refresh token if present, if not, use silent flow.
            LiveConnectSession currentSession = this.AuthClient.LoadSession(this);
            this.scopes = (scopes == null) ? new List<string>() : new List<string>(scopes);

            bool hasRefreshToken = currentSession != null && !string.IsNullOrEmpty(currentSession.RefreshToken);
            if (hasRefreshToken)
            {
                var refreshOp = new RefreshTokenOperation(
                    this,
                    this.clientId,
                    currentSession.RefreshToken,
                    this.scopes,
                    null);

                var tcs = new TaskCompletionSource<LiveLoginResult>();

                try
                {
                    LiveLoginResult refreshOpResult = await refreshOp.ExecuteAsync();
                    this.Session = refreshOpResult.Session;
                    this.AuthClient.SaveSession(this.Session);
                    tcs.TrySetResult(refreshOpResult);
                }
                catch (Exception exception)
                {
                    this.Session = null;
                    tcs.TrySetException(exception);
                }
                finally
                {
                    Interlocked.Decrement(ref this.asyncInProgress);
                }

                return await tcs.Task;
            }

            // If we do NOT have a refresh token, use the silent flow.
            return await this.AuthenticateAsync(true /* silent flow */);
        }

        /// <summary>
        /// Displays the login/consent UI and returns a Session object when user completes the auth flow.
        /// </summary>
        /// <param name="scopes">The list of offers that the application is requesting user consent for.</param>
        /// <returns>A Task object representing the asynchronous operation.</returns>
        public async Task<LiveLoginResult> LoginAsync(IEnumerable<string> scopes)
        {
            if (scopes == null && this.scopes == null)
            {
                throw new ArgumentNullException("scopes");
            }

            if (scopes != null)
            {
                this.scopes = new List<string>(scopes);
            }

            bool onUiThread = Deployment.Current.CheckAccess();
            if (!onUiThread)
            {
                throw new InvalidOperationException(
                    string.Format(ResourceHelper.GetString("NotOnUiThread"), "LoginAsync"));
            }

            this.PrepareForAsync();

            return await this.AuthenticateAsync(false /* silent flow */);
        }

        /// <summary>
        /// Logs user out of the application.  Clears any cached Session data.
        /// </summary>
        public void Logout()
        {
            this.Session = null;
            this.AuthClient.CloseSession();
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Creates a LiveConnectSession object based on the parsed response.
        /// </summary>
        internal static LiveConnectSession CreateSession(LiveAuthClient client, IDictionary<string, object> result)
        {
            var session = new LiveConnectSession(client);

            Debug.Assert(result.ContainsKey(AuthConstants.AccessToken));
            if (result.ContainsKey(AuthConstants.AccessToken))
            {
                session.AccessToken = result[AuthConstants.AccessToken] as string;

                if (result.ContainsKey(AuthConstants.AuthenticationToken))
                {
                    session.AuthenticationToken = result[AuthConstants.AuthenticationToken] as string;
                }

                if (result.ContainsKey(AuthConstants.ExpiresIn))
                {
                    if (result[AuthConstants.ExpiresIn] is string)
                    {
                        session.Expires = LiveAuthClient.CalculateExpiration(result[AuthConstants.ExpiresIn] as string);
                    }
                    else
                    {
                        session.Expires = DateTimeOffset.UtcNow.AddSeconds((int)result[AuthConstants.ExpiresIn]);
                    }
                }

                if (result.ContainsKey(AuthConstants.Scope))
                {
                    session.Scopes =
                        LiveAuthClient.ParseScopeString(HttpUtility.UrlDecode(result[AuthConstants.Scope] as string));
                }

                if (result.ContainsKey(AuthConstants.RefreshToken))
                {
                    session.RefreshToken = result[AuthConstants.RefreshToken] as string;
                }
            }

            return session;
        }

        /// <summary>
        /// Retrieve a new access token based on current session information.
        /// </summary>
        internal bool RefreshToken(Action<LiveLoginResult> completionCallback)
        {
            if (this.Session.IsValid)
            {
                return false;
            }

            var refreshOp = new RefreshTokenOperation(
                this,
                this.clientId,
                this.Session.RefreshToken,
                this.Session.Scopes,
                null)
            {
                OperationCompletedCallback = completionCallback
            };

            refreshOp.Execute();

            return true;
        }

        internal Task<LiveLoginResult> RefreshTokenAsync()
        {
            var refreshOp = new RefreshTokenOperation(
                this,
                this.clientId,
                this.Session.RefreshToken,
                this.Session.Scopes,
                null);

            return refreshOp.ExecuteAsync();
        }

        private Task<LiveLoginResult> AuthenticateAsync(bool useSilentFlow)
        {
            var tcs = new TaskCompletionSource<LiveLoginResult>();

            this.AuthClient.AuthenticateAsync(
                this.clientId,
                LiveAuthClient.BuildScopeString(this.scopes),
                useSilentFlow,
                (string responseData, Exception exception) =>
                {
                    if (exception != null)
                    {
                        tcs.TrySetException(exception);
                        Interlocked.Decrement(ref this.asyncInProgress);
                    }
                    else
                    {
                        this.ProcessAuthResponse(responseData, (LiveLoginResult result) =>
                        {
                            if (result.Error != null)
                            {
                                this.Session = null;
                                tcs.TrySetException(result.Error);
                            }
                            else
                            {
                                this.Session = result.Session;
                                this.AuthClient.SaveSession(this.Session);
                                tcs.TrySetResult(result);
                            }

                            Interlocked.Decrement(ref this.asyncInProgress);
                        });
                    }
                });

            return tcs.Task;
        }

        #endregion
    }
}
