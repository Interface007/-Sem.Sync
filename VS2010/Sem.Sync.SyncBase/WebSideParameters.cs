// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebSideParameters.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines that valiable parts of a web site tobe grabbed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase
{
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Defines that valiable parts of a web site tobe grabbed.
    /// </summary>
    public class WebSideParameters
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the regex to extract the form key for the log on
        /// </summary>
        public string ExtractorFormKey { get; set; }

        /// <summary>
        ///   Gets or sets the regex to extract the iv for the log on
        /// </summary>
        public string ExtractorFriendUrls { get; set; }

        /// <summary>
        ///   Gets or sets the regex to extract the iv for the log on
        /// </summary>
        public string ExtractorIv { get; set; }

        /// <summary>
        ///   Gets or sets the regex to extract the picture url from the profile content
        /// </summary>
        public string ExtractorProfilePictureUrl { get; set; }

        /// <summary>
        ///   Gets or sets the data string to be posted to logon into the site
        /// </summary>
        public string HttpDataLogOnRequest { get; set; }

        /// <summary>
        ///   Gets or sets the detection string to detect if we did fail to logon
        /// </summary>
        public string HttpDetectionStringLogOnFailed { get; set; }

        /// <summary>
        ///   Gets or sets the detection string to parse the content of a request if we need to logon
        /// </summary>
        public string[] HttpDetectionStringLogOnNeeded { get; set; }

        /// <summary>
        ///   Gets or sets the base address to communicate with the site
        /// </summary>
        public string HttpUrlBaseAddress { get; set; }

        /// <summary>
        ///   Gets or sets the url pointing to the contact data to be downloaded - contains a single parameter {0} with the ID from the 
        ///   profile ids for the contact to be processed.
        /// </summary>
        public string HttpUrlContactDownload { get; set; }

        /// <summary>
        ///   Gets or sets the base address to communicate with the site
        /// </summary>
        public string HttpUrlFriendList { get; set; }

        /// <summary>
        ///   Gets or sets the relative url to log on
        /// </summary>
        public string HttpUrlLogOnRequest { get; set; }

        /// <summary>
        ///   Gets or sets the deterministing part of the placeholder url - the url to a placeholder must match this regex, while
        ///   all others must not.
        /// </summary>
        public string ImagePlaceholderUrl { get; set; }

        /// <summary>
        ///   Gets or sets the <see cref = "ProfileIdentifierType" /> of this source.
        /// </summary>
        public ProfileIdentifierType ProfileIdentifierType { get; set; }

        /// <summary>
        /// Gets or sets the url to the list of contact relations
        /// </summary>
        public string ContactListUrl { get; set; }

        /// <summary>
        /// Gets or sets the regular expression to extract a person identifier from a contact relation page
        /// </summary>
        public string PersonIdentifierFromContactsListRegex { get; set; }
        
        /// <summary>
        /// Gest or sets the regular expression that extracts the part of the profile identifier that can be found in the friend list.
        /// This way we can handle profile identifier that do contain redundant information that is not part of the friend list.
        /// </summary>
        public string ProfileIdPartExtractor { get; set; }
        
        /// <summary>
        /// Gets or sets a string that will be used with <see cref="string.Format(string,object[])"/> in order to transform an
        /// extracted indentifier into the format of n external id.
        /// </summary>
        public string ProfileIdFormatter { get; set; }

        #endregion
    }
}