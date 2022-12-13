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
    using System.Diagnostics;
    using System.IO.IsolatedStorage;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Navigation;

    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    /// <summary>
    /// Class that implements the platform specific auth flow for WP7 apps.
    /// It is assumed that the calling code ensures only one AuthenticateAsync
    /// call is active at any given time.
    /// </summary>
    internal class PhoneAuthClient : IAuthClient
    {
        private const int RenderSizeOffset = 32;  // SystemTray size in portait mode.
        private const string True = "true";
        private const string False = "false";

        private Action<string, Exception> callback; 
        private Popup popup;
        private PhoneApplicationPage rootPage;
        private IApplicationBar appBar;
        private readonly LiveAuthClient liveAuthClient;
        private string signingOut;

        /// <summary>
        /// Initialize the PhoneAuthClient object.
        /// </summary>
        public PhoneAuthClient(LiveAuthClient liveAuthClient)
        {
            Debug.Assert(liveAuthClient != null);
            this.liveAuthClient = liveAuthClient;
        }

        /// <summary>
        /// Gets a boolean indicating whether or not the user can be signed out.
        /// For Windows Phone, this is always 'true'.
        /// </summary>
        public bool CanSignOut
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Initialize and invoke the login dialog
        /// </summary>
        /// <param name="clientId">The client id of the application.</param>
        /// <param name="scopes">The scopes that the application needs user consent for.</param>
        /// <param name="silent">True if authentication should be done w/ no UI.</param>
        /// <param name="callback">The callback function to be invoked when login completes.</param>
        public void AuthenticateAsync(string clientId, string scopes, bool silent, Action<string, Exception> callback)
        {
            this.callback = callback;
            
            if (silent)
            {
                // Silent flow not supported on phone.
                if (callback != null)
                {
                    ThreadPool.QueueUserWorkItem(
                        (object state) =>
                        {
                            callback(GenerateUserUnknownResponse(this.liveAuthClient.RedirectUrl), null);
                        });
                }

                return;
            }

            if (this.popup != null)
            {
                throw new InvalidOperationException(ResourceHelper.GetString("LoginPopupAlreadyOpen"));
            }

            string consentUrl = this.liveAuthClient.BuildLoginUrl(scopes, false);

            var rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
            Debug.Assert(rootVisual != null);

            if (rootVisual.RenderSize.Height <= 0)
            {
                throw new InvalidOperationException(ResourceHelper.GetString("RootVisualNotRendered"));
            }

            this.ClearAuthCookieIfNeeded(delegate()
            {
                if (this.IsSigningOut)
                {
                    // We are still signing out?! meaning we probably failed to connect to the network, then fail the request.
                    callback(
                        null,  
                        new LiveAuthException(AuthErrorCodes.ClientError, ResourceHelper.GetString("ConnectionError")));
                    return;
                }

                // Store the application bar and remove from the page so it doesn't interfere with the popup login page.
                // It is restored when the popup closes.
                this.rootPage = rootVisual.Content as PhoneApplicationPage;
                if (this.rootPage != null)
                {
                    this.appBar = rootPage.ApplicationBar;
                    this.rootPage.ApplicationBar = null;
                }

                var loginPage = new LoginPage(consentUrl, this.liveAuthClient.RedirectUrl, this.OnLoginPageCompleted);

                int offset = 0;
                if (SystemTray.IsVisible)
                {
                    offset = PhoneAuthClient.RenderSizeOffset;
                }
                offset = (rootVisual.RenderSize.Height >= offset) ? offset : 0;
                this.popup = new Popup()
                {
                    Child = loginPage,
                    VerticalOffset = offset,
                    Height = Application.Current.RootVisual.RenderSize.Height - offset,
                    IsOpen = true
                };
            });
        }

        /// <summary>
        /// Clear any cached refresh token.
        /// </summary>
        public void CloseSession()
        {
            var appData = IsolatedStorageSettings.ApplicationSettings;
            if (appData.Values.Count > 0)
            {
                if (appData.Contains(LiveAuthClient.StorageConstants.RefreshToken))
                {
                    appData.Remove(LiveAuthClient.StorageConstants.RefreshToken);
                }

                appData.Save();
            }

            this.BeginClearAuthCookie();
        }

        /// <summary>
        /// Load cached refresh token if there is one.
        /// </summary>
        public LiveConnectSession LoadSession(LiveAuthClient authClient)
        {
            LiveConnectSession session = null;
            var appData = IsolatedStorageSettings.ApplicationSettings;
            if (appData.Values.Count > 0)
            {
                session = new LiveConnectSession(authClient);

                if (appData.Contains(LiveAuthClient.StorageConstants.RefreshToken))
                {
                    session.RefreshToken = appData[LiveAuthClient.StorageConstants.RefreshToken] as string;
                }
            }

            return session;
        }

        /// <summary>
        /// Save the current refresh token to cache.
        /// </summary>
        public void SaveSession(LiveConnectSession session)
        {
            if (session != null)
            {
                var appData = IsolatedStorageSettings.ApplicationSettings;
                if (!string.IsNullOrEmpty(session.RefreshToken))
                {
                    if (appData.Contains(LiveAuthClient.StorageConstants.RefreshToken))
                    {
                        appData.Remove(LiveAuthClient.StorageConstants.RefreshToken);
                    }
                    appData.Add(LiveAuthClient.StorageConstants.RefreshToken, session.RefreshToken);
                }

                try
                {
                    appData.Save();
                }
                catch (IsolatedStorageException)
                {
                    // Ignore save exception. It is OK to not update the refresh token every time
                    // since the expiration time of a refresh token is 1 year.
                }
            }
        }

        private string GenerateUserUnknownResponse(string endUrl)
        {
            var sb = new StringBuilder(endUrl);
            sb = sb.Append("#").Append(AuthConstants.Error).Append("=").Append(AuthErrorCodes.UnknownUser);

            return sb.ToString();
        }

        private void OnLoginPageCompleted(string responseData, Exception error)
        {
            if (this.popup != null)
            {
                var rootFrame = Application.Current.RootVisual as Frame;

                Debug.Assert(rootFrame != null);
                rootFrame.Dispatcher.BeginInvoke(
                    () =>
                    {
                        this.popup.IsOpen = false;
                        this.popup = null;

                        if (this.rootPage != null)
                        {
                            this.rootPage.ApplicationBar = this.appBar;
                        }

                        if (this.callback != null)
                        {
                            this.callback(responseData, error);
                        }
                    });
            }
        }

        private bool IsSigningOut
        {
            get
            {
                if (string.IsNullOrEmpty(this.signingOut))
                {
                    // If the data member has not been initialized yet, initialize it by reading from the storage.
                    var appData = IsolatedStorageSettings.ApplicationSettings;
                    var key = LiveAuthClient.StorageConstants.SigningOut;

                    if (appData.Contains(key))
                    {
                        this.signingOut = appData[key] as string;

                        Debug.Assert(this.signingOut == True || this.signingOut == False);
                    }
                    else
                    {
                        this.signingOut = False;
                    }
                }

                return this.signingOut == True;
            }
            set
            {
                string newValue = value ? True : False;
                if (this.signingOut != newValue)
                {
                    this.signingOut = newValue;
                    var appData = IsolatedStorageSettings.ApplicationSettings;
                    appData[LiveAuthClient.StorageConstants.SigningOut] = newValue;

                    try
                    {
                        appData.Save();
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ignore the exception.
                    }
                }
            }
        }

        private Uri SignOutUrl
        {
            get
            {
#if DEBUG
                var authEndpoint = string.IsNullOrEmpty(LiveAuthClient.AuthEndpointOverride) ?
                    LiveAuthClient.DefaultConsentEndpoint : LiveAuthClient.AuthEndpointOverride.TrimEnd('/');
#else
                var authEndpoint = LiveAuthClient.DefaultConsentEndpoint;
#endif
                return new Uri(authEndpoint + AuthEndpointsInfo.LogoutPath);
            }
        }

        private void BeginClearAuthCookie()
        {
            this.IsSigningOut = true;
            this.ClearAuthCookie(null);
        }

        private void ClearAuthCookieIfNeeded(Action callback)
        {
            if (this.IsSigningOut)
            {
                this.ClearAuthCookie(callback);
            }
            else
            {
                callback();
            }
        }

        private void ClearAuthCookie(Action callback)
        {
            WebBrowser browser = new WebBrowser();
            browser.Navigated += (object sender, NavigationEventArgs e) =>
            {
                // We navigated to the sign out page.
                this.IsSigningOut = false;
                if (callback != null)
                {
                    callback();
                }
            };

            browser.NavigationFailed += (object sender, NavigationFailedEventArgs e) =>
            {
                // Navigation failed.
                if (callback != null)
                {
                    callback();
                }
            };

            browser.Navigate(this.SignOutUrl);
        }
    }
}
