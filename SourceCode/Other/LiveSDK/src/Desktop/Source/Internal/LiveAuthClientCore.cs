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
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    
    /// <summary>
    /// LiveAuthClientCore class provides the core implementation of authentication/authorization logic
    /// behind the public LiveAuthClient class
    /// </summary>
    internal class LiveAuthClientCore
    {
        private readonly LiveAuthClient publicAuthClient;
        private readonly string clientId;
        private readonly IRefreshTokenHandler refreshTokenHandler;
        private LiveLoginResult loginStatus;
        private RefreshTokenInfo refreshTokenInfo;
        private TaskCompletionSource<LiveLoginResult> initTask;
        private TaskCompletionSource<LiveConnectSession> codeExchangeTask;
        private IEnumerable<string> initScopes;

        /// <summary>
        /// Initializes a new instance of the LiveAuthClientCore class.
        /// </summary>
        public LiveAuthClientCore(
            string clientId,
            IRefreshTokenHandler refreshTokenHandler,
            LiveAuthClient authClient)
        {
            Debug.Assert(!string.IsNullOrEmpty(clientId));
            Debug.Assert(authClient != null);

            this.clientId = clientId;
            this.refreshTokenHandler = refreshTokenHandler;
            this.publicAuthClient = authClient;
        }

        /// <summary>
        /// Initializes the LiveAuthClient instance by trying to retrieve an access token using refresh token
        /// provided by the app via the IRefreshTokenHandler instance.
        /// </summary>
        public Task<LiveLoginResult> InitializeAsync(IEnumerable<string> scopes)
        {
            // We don't allow InitializeAsync or ExchangeAuthCodeAsync to be invoked concurrently.
            if (this.initTask != null)
            {
                throw new InvalidOperationException(ErrorText.ExistingAuthTaskRunning);
            }

            var task = new TaskCompletionSource<LiveLoginResult>();
            this.initTask = task;
            this.initScopes = scopes;

            if (this.loginStatus != null)
            {
                // We have a result already, then return this one.
                this.OnInitCompleted(null);
            }
            else
            {
                this.loginStatus = new LiveLoginResult(LiveConnectSessionStatus.Unknown, null);
                this.TryRefreshToken();
            }

            return task.Task;
        }

        /// <summary>
        /// Exchange authentication code for access token.
        /// </summary>
        /// <param name="AuthenticationCode">The authentication code the app received from Microsoft authorization server during user authentication and authorization process.</param>
        /// <returns></returns>
        public Task<LiveConnectSession> ExchangeAuthCodeAsync(string authenticationCode)
        {
            Debug.Assert(!string.IsNullOrEmpty(authenticationCode));

            // We don't allow InitializeAsync or ExchangeAuthCodeAsync to be invoked concurrently.
            if (this.codeExchangeTask != null)
            {
                throw new InvalidOperationException(ErrorText.ExistingAuthTaskRunning);
            }

            this.codeExchangeTask = new TaskCompletionSource<LiveConnectSession>();

            this.ExchangeCodeForToken(authenticationCode);

            return this.codeExchangeTask.Task;
        }
        
        /// <summary>
        /// Generates a consent URL that includes a set of provided parameters.
        /// </summary>
        public string GetLoginUrl(IEnumerable<string> scopes, string redirectUrl, DisplayType display, ThemeType theme, string locale, string state)
        {
            return LiveAuthUtility.BuildAuthorizeUrl(this.clientId, redirectUrl, scopes, ResponseType.Code, display, theme, locale, state);
        }

        internal bool CanRefreshToken
        {
            get
            {
                return this.refreshTokenHandler != null;
            }
        }

        internal void TryRefreshToken()
        {
            this.TryRefreshToken(null);
        }

        internal void TryRefreshToken(Action<LiveLoginResult> completionCallback)
        {
            LiveLoginResult result = new LiveLoginResult(LiveConnectSessionStatus.Unknown, null);
            if (this.refreshTokenHandler != null)
            {
                if (this.refreshTokenInfo == null)
                {  
                    this.refreshTokenHandler.RetrieveRefreshTokenAsync().ContinueWith(t =>
                    {
                        this.refreshTokenInfo = t.Result;
                        this.RefreshToken(completionCallback);

                    });
                    return;
                }

                this.RefreshToken(completionCallback);
                return;
            }

            this.OnRefreshTokenCompleted(result, completionCallback);
        }

        private void RefreshToken(Action<LiveLoginResult> completionCallback)
        {
            if (this.refreshTokenInfo != null)
            {
                LiveAuthRequestUtility.RefreshTokenAsync(
                        this.clientId,
                        null,
                        LiveAuthUtility.BuildDesktopRedirectUrl(),
                        this.refreshTokenInfo.RefreshToken,
                        null /*scopes*/
                    ).ContinueWith(t =>
                    {
                        this.OnRefreshTokenCompleted(t.Result, completionCallback);
                    });
            }
            else
            {
                LiveLoginResult result = new LiveLoginResult(LiveConnectSessionStatus.Unknown, null);
                this.OnRefreshTokenCompleted(result, completionCallback);
            }
        }

        private void OnRefreshTokenCompleted(LiveLoginResult result, Action<LiveLoginResult> completionCallback)
        {
            if (completionCallback != null)
            {
                this.UpdateSession(result);
                completionCallback(result);
            }
            else
            {
                this.OnInitCompleted(result);
            }
        }

        private void UpdateSession(LiveLoginResult result)
        {
            Debug.Assert(result != null);

            if (result.Session != null)
            {
                // Set the AuthClient that is needed when refreshing a token.
                result.Session.AuthClient = this.publicAuthClient;

                // We have a new session, update the public property
                this.loginStatus = result;
                this.publicAuthClient.Session = result.Session;

                if (this.refreshTokenHandler != null &&
                    !string.IsNullOrEmpty(result.Session.RefreshToken))
                {
                    RefreshTokenInfo refreshInfo = new RefreshTokenInfo(result.Session.RefreshToken);
                    this.refreshTokenHandler.SaveRefreshTokenAsync(refreshInfo);
                }
            }
            else if (this.loginStatus.Status == LiveConnectSessionStatus.Unknown && 
                result.Status == LiveConnectSessionStatus.NotConnected)
            {
                this.loginStatus = result;
            }
        }

        private void OnInitCompleted(LiveLoginResult authResult)
        {
            authResult = this.ValidateSessionInitScopes(authResult);
            this.UpdateSession(authResult);

            Debug.Assert(this.loginStatus != null);
            this.publicAuthClient.FirePendingPropertyChangedEvents();
            

            if (authResult != null && authResult.Error != null)
            {
                this.initTask.SetException(authResult.Error);
            }
            else
            {
                this.initTask.SetResult(this.loginStatus);
            }

            this.initTask = null;
        }

        private LiveLoginResult ValidateSessionInitScopes(LiveLoginResult loginResult)
        {
            if (loginResult.Session != null && this.initScopes != null)
            {
                if (!LiveAuthUtility.IsSubsetOfScopeRange(this.initScopes, loginResult.Session.Scopes))
                {
                    loginResult = new LiveLoginResult(LiveConnectSessionStatus.NotConnected, null);
                }

                this.initScopes = null;
            }

            return loginResult;
        }

        private void ExchangeCodeForToken(string authorizationCode)
        {
            Task<LiveLoginResult> task = LiveAuthRequestUtility.ExchangeCodeForTokenAsync(
                this.clientId, 
                null, 
                LiveAuthUtility.BuildDesktopRedirectUrl(), 
                authorizationCode);
            task.ContinueWith((Task<LiveLoginResult> t) =>
            {
                this.OnExchangeCodeCompleted(t.Result);
            });
        }

        private void OnExchangeCodeCompleted(LiveLoginResult authResult)
        {
            this.UpdateSession(authResult);

            Debug.Assert(this.loginStatus != null);
            this.publicAuthClient.FirePendingPropertyChangedEvents();
            if (authResult != null && authResult.Error != null)
            {
                this.codeExchangeTask.SetException(authResult.Error);
            }
            else
            {
                this.codeExchangeTask.SetResult(authResult.Session);
            }

            this.codeExchangeTask = null;
        }        
    }
}
