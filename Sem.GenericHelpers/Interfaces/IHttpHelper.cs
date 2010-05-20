// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHttpHelper.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Interface to implementation of HTTP communication class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Interfaces
{
    /// <summary>
    /// Interface to implementation of HTTP communication class
    /// </summary>
    public interface IHttpHelper
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the base address for requests
        /// </summary>
        string BaseUrl { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether a certificate error in an
        ///   https connection whould be ignored.
        ///   debugging using Fiddler needs to bypass certificate approval
        ///   set this const to "true" if you want to enable fiddler-debugging
        ///   this will prevent checking for man in the middle attack
        /// </summary>
        bool IgnoreCertificateError { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to prevent loading missing entries 
        ///   - this is only usefull for debugging purpose if this property is true, we 
        ///   will not download missing content
        /// </summary>
        bool SkipNotCached { get; set; }

        /// <summary>
        ///   Gets or sets the object that is responsible to interact with the user
        /// </summary>
        IUiInteraction UiDispatcher { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to activate caching of content 
        ///   - this will present refreshing the data use this for debugging purpose 
        ///   to not stress the http server
        /// </summary>
        bool UseCache { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use stored ie cookies instead of 
        ///   private session cookies
        /// </summary>
        bool UseIeCookies { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Download content as text
        /// </summary>
        /// <param name="url">
        /// the url to access the content
        /// </param>
        /// <param name="name">
        /// a name for caching - this should correspond to the url
        /// </param>
        /// <returns>
        /// the text result of the request
        /// </returns>
        string GetContent(string url, string name);

        /// <summary>
        /// Download content binary
        /// </summary>
        /// <param name="url">
        /// the url to access the content
        /// </param>
        /// <param name="name">
        /// a name for caching - this should correspond to the url
        /// </param>
        /// <returns>
        /// the binary result of the request without conversion
        /// </returns>
        byte[] GetContentBinary(string url, string name);

        /// <summary>
        /// Download content binary
        /// </summary>
        /// <param name="url">
        /// the url to access the content
        /// </param>
        /// <param name="name">
        /// a name for caching - this should correspond to the url
        /// </param>
        /// <param name="postData">
        /// the data that should be posted to the server
        /// </param>
        /// <returns>
        /// the binary result of the request without conversion
        /// </returns>
        byte[] GetContentBinaryPost(string url, string name, string postData);

        /// <summary>
        /// Download content as text
        /// </summary>
        /// <param name="url">
        /// the url to access the content
        /// </param>
        /// <param name="name">
        /// a name for caching - this should correspond to the url
        /// </param>
        /// <param name="postData">
        /// the complete data to be added (including keys and values) as one string
        /// </param>
        /// <returns>
        /// the text result of the request
        /// </returns>
        string GetContentPost(string url, string name, string postData);

        #endregion
    }
}