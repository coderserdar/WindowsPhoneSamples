// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizedStrings.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. This project was developed by: Sara Silva (saramgsilva@gmail.com)
// </copyright>
// <summary>
//   Provides access to string resources.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AuthenticationSample.WP80.Resources;

namespace AuthenticationSample.WP80
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        private static readonly AppResources AppResources = new AppResources();

        /// <summary>
        /// Gets the localized resources.
        /// </summary>
        /// <value>
        /// The localized resources.
        /// </value>
        public AppResources LocalizedResources
        {
            get { return AppResources; }
        }
    }
}