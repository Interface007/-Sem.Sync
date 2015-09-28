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
    using System.Windows.Forms;

    using Sem.GenericHelpers.Entities;

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

        #region Methods

        /// <summary>
        /// The resolve.
        /// </summary>
        /// <param name="messageForUser"> The message for user. </param>
        /// <param name="title"> The title. </param>
        /// <param name="request"> The request. </param>
        /// <returns> The result structure of a solved catcha </returns>
        internal CaptchaResolveResult Resolve(string messageForUser, string title, CaptchaResolveRequest request)
        {
            if (!string.IsNullOrEmpty(title))
            {
                this.Text = title;
            }

            if (!string.IsNullOrEmpty(messageForUser))
            {
                this.lblMessage.Text = messageForUser;
            }

            return new CaptchaResolveResult { UserReportsSuccess = this.ShowDialog() == DialogResult.OK };
        }

        #endregion
    }
}