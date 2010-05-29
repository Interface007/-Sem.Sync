// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaptchaResolveRequest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Request information to let the user resolve a captcha
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Entities
{
    using System.Drawing;

    /// <summary>
    /// Request information to let the user resolve a captcha
    /// </summary>
    public class CaptchaResolveRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the image to resolve.
        /// </summary>
        public Image CaptchaImage { get; set; }

        /// <summary>
        ///   Gets or sets the url of the web site that will provide the UI to solve the captcha.
        /// </summary>
        public string UrlOfWebSite { get; set; }

        #endregion
    }
}