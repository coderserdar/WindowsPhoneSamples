// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISessionProvider.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   Defines the ISessionProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using AuthenticationSample.WP80.Services.Model;

namespace AuthenticationSample.WP80.Services.Interfaces
{
    /// <summary>
    /// The SessionProvider interface.
    /// </summary>
    public interface ISessionProvider
    {
        /// <summary>
        /// The login sync.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task<Session> LoginAsync();

        /// <summary>
        /// The logout.
        /// </summary>
        void Logout();
    }
}
