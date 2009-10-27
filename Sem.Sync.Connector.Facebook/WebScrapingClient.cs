// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebScrapingClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Facebook
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;

    /// <summary>
    /// WebScaping implementation of a FaceBook StdClient
    /// </summary>
    [ClientStoragePathDescription(
        Irrelevant = true,
        ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(
        DisplayName = "Facebook (WS)",
        Internal = false, 
        CanReadContacts = true, 
        CanWriteContacts = false, 
        NeedsCredentials = true,
        MatchingIdentifier = ProfileIdentifierType.FacebookProfileId)]
    public class WebScrapingClient : WebScrapingBaseClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebScrapingClient"/> class.
        /// </summary>
        public WebScrapingClient()
            : base("http://www.facebook.com/")
        {
        }

        /// <summary>
        /// Gets the friendly name of this class.
        /// </summary>
        public override string FriendlyClientName
        {
            get { return "Facebook-WebScaping-Connector"; }
        }

        /// <summary>
        /// Gets the deterministin part of the placeholder url - the url to a placeholder must match this regex, while
        /// all others must not.
        /// </summary>
        protected override string ImagePlaceholderUrl
        {
            get { return "silhouette.gif"; }
        }

        /// <summary>
        /// Gets the url pointing to the contact data to be downloaded - contains a single parameter {0} with the ID from the 
        /// profile ids for the contact to be processed.
        /// </summary>
        protected override string HttpUrlContactDownload
        {
            get { return "/profile.php?id={0}"; }
        }

        /// <summary>
        /// Gets the <see cref="ProfileIdentifierType"/> of this source.
        /// </summary>
        protected override ProfileIdentifierType ProfileIdentifierType
        {
            get { return ProfileIdentifierType.FacebookProfileId; }
        }

        /// <summary>
        /// Gets a string to detect that we need to authenticate
        /// </summary>
        protected override string HttpDetectionStringLogOnNeeded
        {
            get { return @"""errorSummary"":""Not Logged In"""; }
        }

        /// <summary>
        /// Gets the data for the log on request. This does contain two parameters {0} and {1} for the logon userid {0} and the password {1}.
        /// </summary>
        protected override string HttpDataLogOnRequest
        {
            get { return "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&locale=de_DE&login_ab_group=0&email={0}&pass={1}&pass_placeholder=Passwort&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=WTu8R"; }
        }

        /// <summary>
        /// Gets the extractor regular expression. for the form key to communicate with the server
        /// </summary>
        protected override string ExtractorFormKey
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the extractor regular expression for the "IV" of the form (needed by some web sites).
        /// </summary>
        protected override string ExtractorIv
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the extractor for the urls that do point to the contact profiles.
        /// </summary>
        protected override string ExtractorFriendUrls
        {
            get { return @"""members"":\[((?<id>\d*)[,\]])*"; }
        }

        /// <summary>
        /// Gets the regex to extract the picture url from the profile content
        /// </summary>
        protected override string ExtractorProfilePictureUrl
        {
            get { return @"<img src=""(?<pic>[a-z0-9._/:]*)"" alt=""[a-zA-Z0-9._/: ]*"" id=""profile_pic"""; }
        }

        /// <summary>
        /// Gets a string to detect that the logon attempt was not successfull - in this case we must reauthenticate.
        /// </summary>
        protected override string HttpDetectionStringLogOnFailed
        {
            get { return "no such string available"; }
        }

        /// <summary>
        /// Gets the base address to communicate with the site
        /// </summary>
        protected override string HttpUrlFriendList
        {
            get { return "/friends/ajax/superfriends.php?filter=afp&ref=tn&offset={0}&__a=1 "; }
        }

        /// <summary>
        /// Gets or sets the relative url to log on
        /// </summary>
        protected override string HttpUrlLogOnRequest
        {
            get { return "https://login.facebook.com/login.php?login_attempt=1"; }
        }

        /// <summary>
        /// Converts downloaded data into a StdContact structure.
        /// </summary>
        /// <param name="contactUrl"> The contact url. </param>
        /// <param name="content"> The content. </param>
        /// <returns> a new StdClient created from the data provided </returns>
        protected override StdContact ConvertToStdContact(string contactUrl, string content)
        {
            var result = new StdContact();
            var values = Regex.Matches(content, @"<h1 id=""profile_name"">(.*?)</h1>", RegexOptions.Singleline);

            if (values.Count > 0 && values[0].Groups.Count > 1)
            {
                result.Name = new PersonName(values[0].Groups[1].ToString());
                this.LogProcessingEvent("processing " + result.Name);
            }
            else
            {
                this.LogProcessingEvent("no name found for this contact");
            }

            values = Regex.Matches(content, @"<div class=""(?<key>[^""]*)"" style=""[^""]*""><dt>.*?</dt><dd>(?<value>.*?)</dd></div>", RegexOptions.Singleline);
            foreach (Match match in values)
            {
                var key = match.Groups[1].ToString();
                var value = match.Groups[2].ToString();
                if (value.Contains(">"))
                {
                    value = value.Substring(value.IndexOf('>') + 1);
                }

                if (value.Contains("<"))
                {
                    value = value.Substring(0, value.IndexOf('<'));
                }

                result.InternalSyncData = new SyncData();

                switch (key)
                {
                    case "birthday":
                        result.DateOfBirth = DateTime.Parse(value, CultureInfo.CurrentCulture);
                        break;

                    case "hometown":
                        result.PersonalAddressPrimary = result.PersonalAddressPrimary ?? new AddressDetail();
                        while (value.Contains("  "))
                        {
                            value = value.Replace("  ", " ");
                        }

                        if (Regex.IsMatch(value, "^[0-9]+ "))
                        {
                            result.PersonalAddressPrimary.PostalCode = value.Split(' ')[0];
                            result.PersonalAddressPrimary.CityName = value.Split(' ')[1];
                        }
                        else
                        {
                            result.PersonalAddressPrimary.CityName = value;
                        }

                        break;

                    case "website":
                        result.PersonalHomepage = value;
                        break;

                    case "relationship_status":
                        switch (value)
                        {
                            case "Single":
                                result.RelationshipStatus = RelationshipStatus.Single;
                                break;

                            case "In einer Beziehung mit ":
                            case "In einer Beziehung":
                                result.RelationshipStatus = RelationshipStatus.InARelationship;
                                break;

                            case "Verheiratet mit ":
                            case "Verheiratet":
                                result.RelationshipStatus = RelationshipStatus.Married;
                                break;
                                
                            default:
                                Console.WriteLine(key + " = " + value);
                                break;
                        }

                        break;

                    case "networks":
                        // TODO: add this to the StdContact object model - contains a separated list of freetext
                        break;

                    case "political_views":
                        // TODO: add this to the StdContact object model
                        break;

                    case "religious_views":
                        // TODO: add this to the StdContact object model
                        break;

                    case "siblings":
                        // TODO: add this to the StdContact object model - contains a name
                        break;

                    case "current_city":
                        // this describes the city the person is currently located (different to the home town) - 
                        // we might find a usefull way to deal with it.
                        break;

                    default:
                        Console.WriteLine("new content: " + key + " => " + value);
                        break;
                }
            }

            result.PersonalProfileIdentifiers = new ProfileIdentifiers(this.ProfileIdentifierType, contactUrl.Substring(contactUrl.LastIndexOf("/", StringComparison.Ordinal) + 1));
            return result;
        }
    }
}
