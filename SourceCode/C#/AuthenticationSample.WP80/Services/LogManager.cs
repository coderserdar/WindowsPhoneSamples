// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogManager.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   The log manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Windows;
using AuthenticationSample.WP80.Services.Interfaces;
using Cimbalino.Phone.Toolkit.Services;
using Microsoft.Phone.Controls;

namespace AuthenticationSample.WP80.Services
{
    /// <summary>
    /// The log manager.
    /// </summary>
    public class LogManager : ILogManager
    {
        /// <summary>
        /// The network information service.
        /// </summary>
        private readonly INetworkInformationService _networkInformationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        /// <param name="networkInformationService">The network information service.</param>
        public LogManager(INetworkInformationService networkInformationService)
        {
            _networkInformationService = networkInformationService;
        }

        /// <summary>
        /// The start method for initialize the Log system.
        /// </summary>
        /// <param name="current">
        ///  The current application object.
        /// </param>
        /// <param name="rootFrame">
        ///  The root frame object.
        /// </param>
        /// <param name="key">
        /// The key value for initialize the Log system.
        /// </param>
        public void Init(Application current, PhoneApplicationFrame rootFrame, string key)
        {
           try
            {
                if (_networkInformationService.IsNetworkAvailable)
                {
                  // TODO: use bugsense for exemple, for send reports
                }
            }
           catch (Exception)
           {
               // TODO : Init - Send exception using email, for example
           }
        }

        /// <summary>
        /// The log method.
        /// </summary>
        /// <param name="e">
        /// The exception got.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        public async Task LogAsync(Exception e)
        {
            // TODO: use bugsense for exemple, for send reports
        }

        /// <summary>
        /// The close async method.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        public async Task CloseAsync()
        {
            // TODO: use bugsense for exemple, for send reports
        }
    }
}
