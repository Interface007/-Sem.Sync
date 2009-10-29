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

    using GenericHelpers;

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
        /// Gets the WebSideParameters which defines how to deal with the site.
        /// </summary>
        protected override WebSideParameters WebSideParameters
        {
            get
            {
                return new WebSideParameters
                    {
                        ImagePlaceholderUrl = "silhouette.gif",
                        HttpDetectionStringLogOnNeeded = @"""errorSummary"":""Not Logged In""",
                        HttpUrlContactDownload = "/profile.php?id={0}",
                        ProfileIdentifierType = ProfileIdentifierType.FacebookProfileId,
                        HttpUrlLogOnRequest = "https://login.facebook.com/login.php?login_attempt=1",
                        HttpUrlFriendList = "/friends/ajax/superfriends.php?filter=afp&ref=tn&offset={0}&__a=1",
                        HttpDetectionStringLogOnFailed = "no such string available",
                        ExtractorProfilePictureUrl = @"<img src=""(?<pic>[a-z0-9._/:]*)"" alt=""[a-zA-Z0-9._/: ]*"" id=""profile_pic""",
                        ExtractorFriendUrls = @"""members"":\[((?<id>\d*)[,\]])*",
                        HttpDataLogOnRequest = "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&locale=de_DE&login_ab_group=0&email={0}&pass={1}&pass_placeholder=Passwort&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=WTu8R",
                    };
            }
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
                                Tools.DebugWriteLine(key + " = " + value);
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
                        // TODO: we might find a usefull way to deal with it.
                        break;

                    default:
                        Tools.DebugWriteLine("new content: {0}  => {1}", key, value);
                        break;
                }
            }

            // TODO: find a better way to extract the identifier from the content, so that we don't need to provide te url inside the method signature
            result.PersonalProfileIdentifiers = new ProfileIdentifiers(
                this.WebSideParameters.ProfileIdentifierType, 
                contactUrl.Substring(contactUrl.LastIndexOf("/", StringComparison.Ordinal) + 1));

            // TODO: download the image of the contact [update documentation chapter Facebook]
            return result;
        }
    }
}
