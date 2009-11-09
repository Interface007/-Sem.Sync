// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaptchaResolveRequest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the CaptchaResolveRequest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Entities
{
    /// <summary>
    /// Request information to let the user resolve a captcha
    /// </summary>
    public class CaptchaResolveRequest
    {
        /// <summary>
        /// Gets or sets the url of the web site that will provide the UI to solve the captcha.
        /// </summary>
        public string UrlOfWebSite { get; set; }
    }
}