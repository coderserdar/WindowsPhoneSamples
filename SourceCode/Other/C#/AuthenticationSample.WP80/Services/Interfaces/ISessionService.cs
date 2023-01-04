// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISessionService.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   Defines the ISessionService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using AuthenticationSample.WP80.Services.Model;

namespace AuthenticationSample.WP80.Services.Interfaces
{
    /// <summary>
    /// The SessionService interface.
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// The get session.
        /// </summary>
        /// <returns>
        /// The <see cref="Session"/>.
        /// </returns>
        Session GetSession();
        
        /// <summary>
        /// The login async.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task<bool> LoginAsync(string provider);

        /// <summary>
        /// The logout.
        /// </summary>
        void Logout();
    }
}
