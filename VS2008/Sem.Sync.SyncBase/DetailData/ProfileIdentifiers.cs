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
    using Sem.GenericHelpers;

    /// <summary>
    /// This class implements a set of profile identifiers. One assumption is that each entity does 
    /// have only one identifier of a specific type. This may be a problem in case of Active Directory.
    /// </summary>
    [Serializable]
    public class ProfileIdentifiers : SerializableDictionary<ProfileIdentifierType, ProfileIdInformation>
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
        public ProfileIdentifiers(ProfileIdentifierType type, ProfileIdInformation profileId)
        {
            this.Add(type, profileId);
        }

        /// <summary>
        /// Gets a specific identifier by the type.
        /// </summary>
        /// <param name="type">the type of identifier to read</param>
        /// <returns>the value of the identifier</returns>
        public ProfileIdInformation GetProfileId(ProfileIdentifierType type)
        {
            return this.ContainsKey(type) ? this[type] : null;
        }

        /// <summary>
        /// Sets a specific profile id.
        /// </summary>
        /// <param name="type"> the profile type to set </param>
        /// <param name="newValue"> the new profile id </param>
        /// <returns> The new profile id. </returns>
        public ProfileIdInformation SetProfileId(ProfileIdentifierType type, ProfileIdInformation newValue)
        {
            return this.SetProfileId(type, newValue, false);
        }

        /// <summary>
        /// Sets a specific profile id.
        /// </summary>
        /// <param name="type"> the profile type to set </param>
        /// <param name="newValue"> the new profile id </param>
        /// <param name="dontOverwriteExisting"> If set to true, an existing value will not be overwritten. </param>
        /// <returns> The profile id that does exist in the dictionary after this operation. </returns>
        public ProfileIdInformation SetProfileId(ProfileIdentifierType type, ProfileIdInformation newValue, bool dontOverwriteExisting)
        {
            if (this.ContainsKey(type))
            {
                if (dontOverwriteExisting)
                {
                    return this[type];
                }

                this[type] = newValue;
                return newValue;
            }

            this.Add(type, newValue);

            return newValue;
        }

        /// <summary>
        /// Tests if any of the identifiers provided with the <paramref name="other"/> parameter 
        /// does match to this set of identifiers.
        /// </summary>
        /// <param name="other">the set to test for</param>
        /// <returns>true in case of min. one matches</returns>
        public bool MatchesAny(ProfileIdentifiers other)
        {
            foreach (var identifier in other)
            {
                if (this.GetProfileId(identifier.Key) == identifier.Value)
                {
                    return true;
                }
            }

            return false;

            ////return (other == null)
            ////    ? false
            ////    : (!string.IsNullOrEmpty(this.XingNameProfileId) && this.XingNameProfileId == other.XingNameProfileId) ||
            ////      (!string.IsNullOrEmpty(this.ActiveDirectoryId) && this.ActiveDirectoryId == other.ActiveDirectoryId) ||
            ////      (!string.IsNullOrEmpty(this.MicrosoftAccessId) && this.MicrosoftAccessId == other.MicrosoftAccessId) ||
            ////      (!string.IsNullOrEmpty(this.WerKenntWenUrl) && this.WerKenntWenUrl == other.WerKenntWenUrl) ||
            ////      (!string.IsNullOrEmpty(this.FacebookProfileId) && this.FacebookProfileId == other.FacebookProfileId) ||
            ////      (!string.IsNullOrEmpty(this.MeinVZPersonId) && this.MeinVZPersonId == other.MeinVZPersonId) ||
            ////      (!string.IsNullOrEmpty(this.StayFriendsPersonId) && this.StayFriendsPersonId == other.StayFriendsPersonId) ||
            ////      (!string.IsNullOrEmpty(this.LinkedInId) && this.LinkedInId == other.LinkedInId) ||
            ////      (!string.IsNullOrEmpty(this.GoogleId) && this.GoogleId == other.GoogleId) ||
            ////      (!string.IsNullOrEmpty(this.LotusNotesId) && this.LotusNotesId == other.LotusNotesId) ||
            ////      (!string.IsNullOrEmpty(this.OracleCrmOnDemandId) && this.OracleCrmOnDemandId == other.OracleCrmOnDemandId) ||
            ////      (!string.IsNullOrEmpty(this.ExchangeWs) && this.ExchangeWs == other.ExchangeWs) ||
            ////      (!string.IsNullOrEmpty(this.DefaultProfileId) && this.DefaultProfileId == other.DefaultProfileId);
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

            foreach (var identifier in this)
            {
                result.Append(" - ");
                result.Append(identifier.Value);
            }

            var resultString = result.ToString();
            return resultString.Length > 3 ? resultString.Substring(3) : string.Empty;
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

        protected override ProfileIdInformation CreateNewValueItem(string value)
        {
            return value;
        }

        protected override string TranslateKey(string keyName)
        {
            switch (keyName)
            {
                case "DefaultProfileId":
                    return "Default";

                case "MeinVZPersonId":
                    return "MeinVZ";

                case "GoogleId":
                    return "Google";
            }

            return base.TranslateKey(keyName);
        }
    }
}