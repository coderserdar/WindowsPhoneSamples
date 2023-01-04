// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogManager.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   Defines the ILogManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace AuthenticationSample.WP80.Services.Interfaces
{
    /// <summary>
    /// The LogManager interface.
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// The start method for initialize the Log system.
        /// </summary>
        /// <param name="current">
        /// The current.
        /// </param>
        /// <param name="rootFrame">
        /// The root Frame.
        /// </param>
        /// <param name="key">
        /// The key value for initialize the Log system.
        /// </param>
        void Init(System.Windows.Application current, Microsoft.Phone.Controls.PhoneApplicationFrame rootFrame, string key);

        /// <summary>
        /// The log method.
        /// </summary>
        /// <param name="e">
        /// The exception got.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task LogAsync(Exception e);

        /// <summary>
        /// The close async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task CloseAsync();
    }
}
