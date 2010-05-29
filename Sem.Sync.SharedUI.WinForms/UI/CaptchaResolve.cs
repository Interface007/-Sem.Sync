// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaptchaResolve.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The captcha resolve.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.UI
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Entities;
    using Sem.GenericHelpers.Interfaces;

    /// <summary>
    /// The captcha resolve.
    /// </summary>
    public partial class CaptchaResolve : Form
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaResolve"/> class.
        /// </summary>
        public CaptchaResolve()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Page.
        /// </summary>
        protected string Page { get; set; }

        /// <summary>
        /// Gets or sets Requester.
        /// </summary>
        protected IHttpHelper Requester { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The resolve.
        /// </summary>
        /// <param name="messageForUser">
        /// The message for user.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// </returns>
        internal CaptchaResolveResult Resolve(
            string messageForUser, string title, CaptchaResolveRequest request)
        {
            if (!string.IsNullOrEmpty(title))
            {
                this.Text = title;
            }

            if (!string.IsNullOrEmpty(messageForUser))
            {
                this.lblMessage.Text = messageForUser;
            }

            this.Requester = request.HttpHelper;

            this.Page = this.Requester.GetContent(request.UrlOfWebSite);
            var imageStream = new MemoryStream(this.Requester.GetContentBinary(GetImageFromPage(this.Page)));
            this.picCaptcha.Image = Image.FromStream(imageStream);
            imageStream.Dispose();

            return new CaptchaResolveResult { UserReportsSuccess = this.ShowDialog() == DialogResult.OK };
        }

        /// <summary>
        /// The get image from page.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// The get image from page.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private static string GetImageFromPage(string page)
        {
            var imageUrl = System.Text.RegularExpressions.Regex.Match(
                page, "<iframe src=\"(http://api.recaptcha.net/noscript[?]k=[a-zA-Z0-9]*)");
            if (imageUrl.Groups.Count == 2)
            {
                return imageUrl.Groups[1].ToString();
            }

            throw new NotImplementedException();
        }

        #endregion
    }
}