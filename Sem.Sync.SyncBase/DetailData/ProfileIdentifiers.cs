//-----------------------------------------------------------------------
// <copyright file="ProfileIdentifiers.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.Text;

    /// <summary>
    /// This class implements a set of profile identifiers. One assumption is that each entity does 
    /// have only one identifier of a specific type. This may be a problem in case of Active Directory.
    /// </summary>
    [Serializable]
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

                case ProfileIdentifierType.OracleCrmOnDemandId:
                    this.OracleCrmOnDemandId = profileId;
                    break;

                case ProfileIdentifierType.MicrosoftAccessId:
                    this.MicrosoftAccessId = profileId;
                    break;

                case ProfileIdentifierType.FacebookProfileId:
                    this.FacebookProfileId = profileId;
                    break;

                case ProfileIdentifierType.XingNameProfileId:
                    this.XingNameProfileId = profileId;
                    break;

                case ProfileIdentifierType.WerKenntWenUrl:
                    this.WerKenntWenUrl = profileId;
                    break;

                case ProfileIdentifierType.StayFriendsPersonId:
                    this.StayFriendsPersonId = profileId;
                    break;

                case ProfileIdentifierType.LinkedInId:
                    this.LinkedInId = profileId;
                    break;

                case ProfileIdentifierType.MeinVZ:
                    this.MeinVZPersonId = profileId;
                    break;

                case ProfileIdentifierType.Google:
                    this.GoogleId = profileId;
                    break;

                case ProfileIdentifierType.LotusNotesId:
                    this.LotusNotesId = profileId;
                    break;

                default:
                    this.DefaultProfileId = profileId;
                    break;
            }
        }

        /// <summary>
        /// Gets or sets the profile id of the persons xing membership.
        /// This defaults to the contact elements interlan Id (a Guid)
        /// </summary>
        public ProfileIdInformation DefaultProfileId { get; set; }

        /// <summary>
        /// Gets or sets the profile id of the persons xing membership. This property is
        /// obsolete now, because the ID is not stable/unique.
        /// </summary>
        [Obsolete("This property did host the ID that is included inside the vCards exported from Xing. This ID seem to be not unique. Use XingNameProfileId instead.")]
        public ProfileIdInformation XingProfileId { get; set; }

        /// <summary>
        /// Gets or sets the profile name of the persons xing membership.
        /// </summary>
        public ProfileIdInformation XingNameProfileId { get; set; }

        /// <summary>
        /// Gets or sets the profile name of the persons xing membership.
        /// </summary>
        public ProfileIdInformation LotusNotesId { get; set; }

        /// <summary>
        /// Gets or sets the id of the persons Facebook profile.
        /// </summary>
        public ProfileIdInformation FacebookProfileId { get; set; }

        /// <summary>
        /// Gets or sets a full qualified active directory user name (including the domain)
        /// </summary>
        public ProfileIdInformation ActiveDirectoryId { get; set; }

        /// <summary>
        /// Gets or sets an id for Oracle Crm on Demand
        /// </summary>
        public ProfileIdInformation OracleCrmOnDemandId { get; set; }

        /// <summary>
        /// Gets or sets a full qualified active directory user name (including the domain)
        /// </summary>
        public ProfileIdInformation MicrosoftAccessId { get; set; }

        /// <summary>
        /// Gets or sets the url at the social network side Wer-Kennt-Wen.de
        /// </summary>
        public ProfileIdInformation WerKenntWenUrl { get; set; }

        /// <summary>
        /// Gets or sets the url at the social network side StayFriends.de
        /// </summary>
        public ProfileIdInformation StayFriendsPersonId { get; set; }

        /// <summary>
        /// Gets or sets the url at the social network side StayFriends.de
        /// </summary>
        public ProfileIdInformation LinkedInId { get; set; }

        /// <summary>
        /// Gets or sets the url at the social network side StayFriends.de
        /// </summary>
        public ProfileIdInformation MeinVZPersonId { get; set; }

        /// <summary>
        /// Gets or sets the google user id
        /// </summary>
        public ProfileIdInformation GoogleId { get; set; }

        /// <summary>
        /// Gets a specific identifier by the type.
        /// </summary>
        /// <param name="type">the type of identifier to read</param>
        /// <returns>the value of the identifier</returns>
        public string GetProfileId(ProfileIdentifierType type)
        {
            string result;

            switch (type)
            {
                case ProfileIdentifierType.ActiveDirectoryId:
                    result = this.ActiveDirectoryId;
                    break;

                case ProfileIdentifierType.OracleCrmOnDemandId:
                    result = this.OracleCrmOnDemandId;
                    break;

                case ProfileIdentifierType.MicrosoftAccessId:
                    result = this.MicrosoftAccessId;
                    break;

                case ProfileIdentifierType.FacebookProfileId:
                    result = this.FacebookProfileId;
                    break;

                case ProfileIdentifierType.XingNameProfileId:
                    result = this.XingNameProfileId;
                    break;

                case ProfileIdentifierType.LotusNotesId:
                    result = this.LotusNotesId;
                    break;

                case ProfileIdentifierType.WerKenntWenUrl:
                    result = this.WerKenntWenUrl;
                    break;

                case ProfileIdentifierType.StayFriendsPersonId:
                    result = this.StayFriendsPersonId;
                    break;

                case ProfileIdentifierType.LinkedInId:
                    result = this.LinkedInId;
                    break;

                case ProfileIdentifierType.MeinVZ:
                    result = this.MeinVZPersonId;
                    break;

                case ProfileIdentifierType.Google:
                    result = this.GoogleId;
                    break;

                default:
                    result = this.DefaultProfileId;
                    break;
            }

            return result;
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

                case ProfileIdentifierType.OracleCrmOnDemandId:
                    this.OracleCrmOnDemandId = newValue;
                    break;

                case ProfileIdentifierType.MicrosoftAccessId:
                    this.MicrosoftAccessId = newValue;
                    break;

                case ProfileIdentifierType.FacebookProfileId:
                    this.FacebookProfileId = newValue;
                    break;

                case ProfileIdentifierType.XingNameProfileId:
                    this.XingNameProfileId = newValue;
                    break;

                case ProfileIdentifierType.WerKenntWenUrl:
                    this.WerKenntWenUrl = newValue;
                    break;

                case ProfileIdentifierType.StayFriendsPersonId:
                    this.StayFriendsPersonId = newValue;
                    break;

                case ProfileIdentifierType.LinkedInId:
                    this.LinkedInId = newValue;
                    break;

                case ProfileIdentifierType.MeinVZ:
                    this.MeinVZPersonId = newValue;
                    break;

                case ProfileIdentifierType.Google:
                    this.GoogleId = newValue;
                    break;

                case ProfileIdentifierType.LotusNotesId:
                    this.LotusNotesId = newValue;
                    break;

                default:
                    this.DefaultProfileId = newValue;
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
                : (!string.IsNullOrEmpty(this.XingNameProfileId) && this.XingNameProfileId == other.XingNameProfileId) ||
                  (!string.IsNullOrEmpty(this.ActiveDirectoryId) && this.ActiveDirectoryId == other.ActiveDirectoryId) ||
                  (!string.IsNullOrEmpty(this.MicrosoftAccessId) && this.MicrosoftAccessId == other.MicrosoftAccessId) ||
                  (!string.IsNullOrEmpty(this.WerKenntWenUrl) && this.WerKenntWenUrl == other.WerKenntWenUrl) ||
                  (!string.IsNullOrEmpty(this.FacebookProfileId) && this.FacebookProfileId == other.FacebookProfileId) ||
                  (!string.IsNullOrEmpty(this.MeinVZPersonId) && this.MeinVZPersonId == other.MeinVZPersonId) ||
                  (!string.IsNullOrEmpty(this.StayFriendsPersonId) && this.StayFriendsPersonId == other.StayFriendsPersonId) ||
                  (!string.IsNullOrEmpty(this.LinkedInId) && this.LinkedInId == other.LinkedInId) ||
                  (!string.IsNullOrEmpty(this.GoogleId) && this.GoogleId == other.GoogleId) ||
                  (!string.IsNullOrEmpty(this.LotusNotesId) && this.LotusNotesId == other.LotusNotesId) ||
                  (!string.IsNullOrEmpty(this.OracleCrmOnDemandId) && this.OracleCrmOnDemandId == other.OracleCrmOnDemandId) ||
                  (!string.IsNullOrEmpty(this.DefaultProfileId) && this.DefaultProfileId == other.DefaultProfileId);
        }

        /// <summary>
        /// Implements a comparison to determine if the provile identifier describes the same person. This means if ANY of the
        /// indentifiers is equal to the other object.
        /// </summary>
        /// <param name="other"> The "other" object. </param>
        /// <returns> true if one of the identifiers matches </returns>
        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != typeof(ProfileIdentifiers))
            {
                return false;
            }

            return this.Equals((ProfileIdentifiers)other);
        }

        /// <summary>
        /// Implements a comparison to determine if the provile identifier describes the same person. This means if ANY of the
        /// indentifiers is equal to the other object.
        /// </summary>
        /// <param name="other">the "other" object</param>
        /// <returns>true if one of the identifiers matches</returns>
        public bool Equals(ProfileIdentifiers other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.MatchesAny(other);
        }

        /// <summary>
        /// Overrides the ToString implementation by returning a meaningful string
        /// </summary>
        /// <returns> a string containing the ids </returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append((this.FacebookProfileId ?? string.Empty) + " - ");
            result.Append((this.ActiveDirectoryId ?? string.Empty) + " - ");
            result.Append((this.MicrosoftAccessId ?? string.Empty) + " - ");
            result.Append((this.WerKenntWenUrl ?? string.Empty) + " - ");
            result.Append((this.MeinVZPersonId ?? string.Empty) + " - ");
            result.Append((this.StayFriendsPersonId ?? string.Empty) + " - ");
            result.Append((this.LinkedInId ?? string.Empty) + " - ");
            result.Append((this.GoogleId ?? string.Empty) + " - ");
            result.Append((this.XingNameProfileId ?? string.Empty) + " - ");
            result.Append((this.LotusNotesId ?? string.Empty) + " - ");
            result.Append((this.OracleCrmOnDemandId ?? string.Empty) + " - ");
            result.Append((this.DefaultProfileId ?? string.Empty));
            return result.ToString();
        }

        /// <summary>
        /// overrides the GetHashCode to be sure all identifiers are compared
        /// </summary>
        /// <returns> this class returns always 0 </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                // enforce explicit comparison of identifiers
                return 0;
            }
        }
    }
}