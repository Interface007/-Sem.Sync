using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sem.GenericHelpers;
using Sem.GenericHelpers.Entities;

namespace Sem.Sync.SharedUI.WinForms.UI
{
    public partial class CaptchaResolve : Form
    {
        public CaptchaResolve()
        {
            InitializeComponent();
        }

        internal Sem.GenericHelpers.Entities.CaptchaResolveResult Resolve(string messageForUser, string title, CaptchaResolveRequest request)
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
            var imageStream = new System.IO.MemoryStream(this.Requester.GetContentBinary(this.GetImageFromPage(this.Page)));
            this.picCaptcha.Image = Image.FromStream(imageStream);
            imageStream.Dispose();

            return new CaptchaResolveResult { UserReportsSuccess = this.ShowDialog() == DialogResult.OK };
        }

        protected string Page { get; set; }

        private string GetImageFromPage(string page)
        {
            var imageUrl = System.Text.RegularExpressions.Regex.Match(page, "<iframe src=\"(http://api.recaptcha.net/noscript[?]k=[a-zA-Z0-9]*)");
            if (imageUrl.Groups.Count == 2)
            {
                return imageUrl.Groups[1].ToString();
            }
            throw new NotImplementedException();
        }

        protected HttpHelper Requester { get; set; }
    }
}
