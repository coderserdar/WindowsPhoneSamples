// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionService.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   Defines the ServiceSession type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AuthenticationSample.WP80.Resources;
using AuthenticationSample.WP80.Services.Interfaces;
using AuthenticationSample.WP80.Services.Model;
using Cimbalino.Phone.Toolkit.Services;

namespace AuthenticationSample.WP80.Services
{
    /// <summary>
    /// The service session.
    /// </summary>
    public class SessionService : ISessionService
    {
        private readonly IApplicationSettingsService _applicationSettings;
        private readonly IFacebookService _facebookService;
        private readonly IMicrosoftService _microsoftService;
        private readonly IGoogleService _googleService;
        private readonly ILogManager _logManager;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionService" /> class.
        /// </summary>
        /// <param name="applicationSettings">The application settings.</param>
        /// <param name="facebookService">The facebook service.</param>
        /// <param name="microsoftService">The microsoft service.</param>
        /// <param name="googleService">The google service.</param>
        /// <param name="logManager">The log manager.</param>
        public SessionService(IApplicationSettingsService applicationSettings,
            IFacebookService facebookService,
            IMicrosoftService microsoftService,
            IGoogleService googleService, ILogManager logManager)
        {
            _applicationSettings = applicationSettings;
            _facebookService = facebookService;
            _microsoftService = microsoftService;
            _googleService = googleService;
            _logManager = logManager;
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns>The session object.</returns>
        public Session GetSession()
        {
            var expiryValue = DateTime.MinValue;
            string expiryTicks = LoadEncryptedSettingValue("session_expiredate");
            if (!string.IsNullOrWhiteSpace(expiryTicks))
            {
                long expiryTicksValue;
                if (long.TryParse(expiryTicks, out expiryTicksValue))
                {
                    expiryValue = new DateTime(expiryTicksValue);
                }
            }

            var session = new Session
            {
                AccessToken = LoadEncryptedSettingValue("session_token"),
                Id = LoadEncryptedSettingValue("session_id"),
                ExpireDate = expiryValue,
                Provider = LoadEncryptedSettingValue("session_provider")
            };

            _applicationSettings.Set(Constants.LoginToken, true);
            _applicationSettings.Save();
            return session;
        }

        /// <summary>
        /// The save session.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        private void Save(Session session)
        {
            SaveEncryptedSettingValue("session_token", session.AccessToken);
            SaveEncryptedSettingValue("session_id", session.Id);
            SaveEncryptedSettingValue("session_expiredate", session.ExpireDate.Ticks.ToString(CultureInfo.InvariantCulture));
            SaveEncryptedSettingValue("session_provider", session.Provider);
            _applicationSettings.Set(Constants.LoginToken, true);
            _applicationSettings.Save();
        }

        /// <summary>
        /// The clean session.
        /// </summary>
        private void CleanSession()
        {
            _applicationSettings.Reset("session_token");
            _applicationSettings.Reset("session_id");
            _applicationSettings.Reset("session_expiredate");
            _applicationSettings.Reset("session_provider");
            _applicationSettings.Reset(Constants.LoginToken);
            _applicationSettings.Save();
        }

        /// <summary>
        /// The login async.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        public async Task<bool> LoginAsync(string provider)
        {
            Exception exception = null;
            try
            {
                Session session = null;
                switch (provider)
                {
                    case Constants.FacebookProvider:
                        session = await _facebookService.LoginAsync();
                        break;
                    case Constants.MicrosoftProvider:
                        session = await _microsoftService.LoginAsync();
                        break;
                    case Constants.GoogleProvider:
                        session = await _googleService.LoginAsync();
                        break;
                }
                if (session != null)
                {
                    Save(session);
                }

                return true;
            }
            catch (InvalidOperationException e)
            {
                throw;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            await _logManager.LogAsync(exception);

            return false;
        }

        /// <summary>
        /// The logout.
        /// </summary>
        public async void Logout()
        {
            Exception exception = null;
            try
            {
                var session = GetSession();
                switch (session.Provider)
                {
                    case Constants.FacebookProvider:
                        _facebookService.Logout();
                        break;
                    case Constants.MicrosoftProvider:
                        _microsoftService.Logout();
                        break;
                    case Constants.GoogleProvider:
                        _googleService.Logout();
                        break;
                }
                CleanSession();
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
        /// Loads an encrypted setting value for a given key.
        /// </summary>
        /// <param name="key">
        /// The key to load.
        /// </param>
        /// <returns>
        /// The value of the key.
        /// </returns>
        private string LoadEncryptedSettingValue(string key)
        {
            string value = null;

            var protectedBytes = _applicationSettings.Get<byte[]>(key);
            if (protectedBytes != null)
            {
                byte[] valueBytes = ProtectedData.Unprotect(protectedBytes, null);
                value = Encoding.UTF8.GetString(valueBytes, 0, valueBytes.Length);
            }

            return value;
        }

        /// <summary>
        /// Saves a setting value against a given key, encrypted.
        /// </summary>
        /// <param name="key">
        /// The key to save against.
        /// </param>
        /// <param name="value">
        /// The value to save against.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The key or value provided is unexpected.
        /// </exception>
        private void SaveEncryptedSettingValue(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                // Encrypt the value by using the Protect() method.
                byte[] protectedBytes = ProtectedData.Protect(valueBytes, null);
                _applicationSettings.Set(key, protectedBytes);
                _applicationSettings.Save();
            }
        }
    }
}
