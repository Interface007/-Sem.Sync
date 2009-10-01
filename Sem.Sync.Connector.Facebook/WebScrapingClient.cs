// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebScapingClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Facebook
{
    /// <summary>
    /// WebScaping implementation of a FaceBook StdClient
    /// </summary>
    public class WebScrapingClient : WebScrapingBaseClient
    {
        protected override string HttpDetectionStringLogOnNeeded
        {
            get { throw new System.NotImplementedException(); }
        }

        protected override string HttpDataLogOnRequest
        {
            get { throw new System.NotImplementedException(); }
        }

        protected override string ExtractorFormKey
        {
            get { throw new System.NotImplementedException(); }
        }

        protected override string ExtractorIv
        {
            get { throw new System.NotImplementedException(); }
        }

        protected override string ExtractorFriendUrls
        {
            get { throw new System.NotImplementedException(); }
        }

        protected override string ContactContentSelector
        {
            get { throw new System.NotImplementedException(); }
        }

        public override string FriendlyClientName
        {
            get
            {
                return "Facebook-WebScaping";
            }
        }
    }
}
