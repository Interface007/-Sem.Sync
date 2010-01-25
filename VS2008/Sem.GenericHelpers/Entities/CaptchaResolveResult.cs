// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaptchaResolveResult.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the CaptchaResolveResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Entities
{
    /// <summary>
    /// Result from a Captach Resolve Request.
    /// </summary>
    public class CaptchaResolveResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user did report a success in resolving the Captcha.
        /// </summary>
        public bool UserReportsSuccess { get; set; }
    }
}