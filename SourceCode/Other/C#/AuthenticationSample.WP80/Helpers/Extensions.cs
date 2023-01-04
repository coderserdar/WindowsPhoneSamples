// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   Defines the Extensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Cimbalino.Phone.Toolkit.Services;

namespace AuthenticationSample.WP80.Helpers
{
    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Removes all back stack.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        public static void RemoveAllBackStack(this INavigationService navigationService)
        {
            while (navigationService.CanGoBack)
            {
                navigationService.RemoveBackEntry();
            }
        }
    }
}
