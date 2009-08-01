//-----------------------------------------------------------------------
// <copyright file="ProfileIdentifiers.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// Identifies the target system for the profile identifier
    /// </summary>
    public enum ProfileIdentifierType
    {
        /// <summary>
        /// unknown or not registered target system
        /// </summary>
        Default = 0,

        /// <summary>
        /// target system: www.xing.com
        /// </summary>
        XingProfileId,

        /// <summary>
        /// target system: www.facebook.com
        /// </summary>
        FacebookProfileId,

        /// <summary>
        /// An active directory - this should include an official DNS name.
        /// </summary>
        ActiveDirectoryId,

        /// <summary>
        /// this is the url at the social network side Wer-Kennt-Wen.de.
        /// </summary>
        WerKenntWenUrl,

        /// <summary>
        /// this is a string identifying the contact at the social network side MeinVZ.net.
        /// </summary>
        MeinVZ,
    }

    /// <summary>
    /// This class implements a set of profile identifiers. One assumption is that each entity does 
    /// have only one identifier of a specific type. This may be a problem in case of Active Directory.
    /// </summary>
    public class ProfileIdentifiers
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileIdentifiers"/> class. 
        /// </summary>
        public ProfileIdentifiers()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileIdentifiers"/> class and
        /// sets one of the identifiers.
        /// </summary>
        /// <param name="type">the identifier to set</param>
        /// <param name="profileId">the value to set for the identifier</param>
        public ProfileIdentifiers(ProfileIdentifierType type, string profileId)
        {
            switch (type)
            {
                case ProfileIdentifierType.ActiveDirectoryId:
                    this.ActiveDirectoryId = profileId;
                    break;

                case ProfileIdentifierType.FacebookProfileId:
                    this.FacebookProfileId = profileId;
                    break;

                case ProfileIdentifierType.XingProfileId:
                    this.XingProfileId = profileId;
                    break;

                case ProfileIdentifierType.WerKenntWenUrl:
                    this.WerKenntWenUrl = profileId;
                    break;
            }
        }

        /// <summary>
        /// Gets or sets the profile id of the persons xing membership.
        /// </summary>
        public string XingProfileId { get; set; }

        /// <summary>
        /// Gets or sets the id of the persons Facebook profile.
        /// </summary>
        public string FacebookProfileId { get; set; }

        /// <summary>
        /// Gets or sets a full qualified active directory user name (including the domain)
        /// </summary>
        public string ActiveDirectoryId { get; set; }

        /// <summary>
        /// Gets or sets the url at the social network side Wer-Kennt-Wen.de
        /// </summary>
        public string WerKenntWenUrl { get; set; }

        /// <summary>
        /// Gets a specific identifier by the type.
        /// </summary>
        /// <param name="type">the type of identifier to read</param>
        /// <returns>the value of the identifier</returns>
        public string GetProfileId(ProfileIdentifierType type)
        {
            switch (type)
            {
                case ProfileIdentifierType.ActiveDirectoryId:
                    return this.ActiveDirectoryId;

                case ProfileIdentifierType.FacebookProfileId:
                    return this.FacebookProfileId;

                case ProfileIdentifierType.XingProfileId:
                    return this.XingProfileId;
                
                case ProfileIdentifierType.WerKenntWenUrl:
                    return this.WerKenntWenUrl;
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets a specific profile id.
        /// </summary>
        /// <param name="type">the profile type to set</param>
        /// <param name="newValue">the new profile id</param>
        public void SetProfileId(ProfileIdentifierType type, string newValue)
        {
            switch (type)
            {
                case ProfileIdentifierType.ActiveDirectoryId:
                    this.ActiveDirectoryId = newValue;
                    break;

                case ProfileIdentifierType.FacebookProfileId:
                    this.FacebookProfileId = newValue;
                    break;

                case ProfileIdentifierType.XingProfileId:
                    this.XingProfileId = newValue;
                    break;

                case ProfileIdentifierType.WerKenntWenUrl:
                    this.WerKenntWenUrl = newValue;
                    break;
            }
        }

        /// <summary>
        /// Tests if any of the identifiers provided with the <paramref name="other"/> parameter 
        /// does match to this set of identifiers.
        /// </summary>
        /// <param name="other">the set to test for</param>
        /// <returns>true in case of min. one matches</returns>
        public bool MatchesAny(ProfileIdentifiers other)
        {
            return (other == null)
                ? false
                : (!string.IsNullOrEmpty(this.XingProfileId) && this.XingProfileId == other.XingProfileId) ||
                  (!string.IsNullOrEmpty(this.ActiveDirectoryId) && this.ActiveDirectoryId == other.ActiveDirectoryId) ||
                  (!string.IsNullOrEmpty(this.WerKenntWenUrl) && this.WerKenntWenUrl == other.WerKenntWenUrl) ||
                  (!string.IsNullOrEmpty(this.FacebookProfileId) && this.FacebookProfileId == other.FacebookProfileId);
        }
    }
}