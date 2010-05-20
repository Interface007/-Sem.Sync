// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProfileIdentifiers.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class implements a set of profile identifiers. One assumption is that each entity does
//   have only one identifier of a specific type. This may be a problem in case of Active Directory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.Linq;
    using System.Text;

    using Sem.GenericHelpers;

    /// <summary>
    /// This class implements a set of profile identifiers. One assumption is that each entity does 
    ///   have only one identifier of a specific type. This may be a problem in case of Active Directory.
    /// </summary>
    [Serializable]
    public class ProfileIdentifierDictionary : SerializableDictionary<ProfileIdentifierType, ProfileIdInformation>
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ProfileIdentifierDictionary" /> class.
        /// </summary>
        public ProfileIdentifierDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileIdentifierDictionary"/> class and
        ///   sets one of the identifiers.
        /// </summary>
        /// <param name="type">
        /// the identifier to set
        /// </param>
        /// <param name="profileId">
        /// the value to set for the identifier
        /// </param>
        public ProfileIdentifierDictionary(ProfileIdentifierType type, ProfileIdInformation profileId)
        {
            this.Add(type, profileId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Implements a comparison to determine if the provile identifier describes the same person. This means if ANY of the
        ///   indentifiers is equal to the other object.
        /// </summary>
        /// <param name="obj">
        /// The "other" object. 
        /// </param>
        /// <returns>
        /// true if one of the identifiers matches 
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(ProfileIdentifierDictionary))
            {
                return false;
            }

            return this.Equals((ProfileIdentifierDictionary)obj);
        }

        /// <summary>
        /// Implements a comparison to determine if the provile identifier describes the same person. This means if ANY of the
        ///   indentifiers is equal to the other object.
        /// </summary>
        /// <param name="other">
        /// the "other" object
        /// </param>
        /// <returns>
        /// true if one of the identifiers matches
        /// </returns>
        public bool Equals(ProfileIdentifierDictionary other)
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
        /// overrides the GetHashCode to be sure all identifiers are compared
        /// </summary>
        /// <returns>
        /// this class returns always 0 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                // enforce explicit comparison of identifiers
                return 0;
            }
        }

        /// <summary>
        /// Gets a specific identifier by the type.
        /// </summary>
        /// <param name="type">
        /// the type of identifier to read
        /// </param>
        /// <returns>
        /// the value of the identifier
        /// </returns>
        public ProfileIdInformation GetProfileId(ProfileIdentifierType type)
        {
            return this.ContainsKey(type) ? this[type] : null;
        }

        /// <summary>
        /// Tests if any of the identifiers provided with the <paramref name="other"/> parameter 
        ///   does match to this set of identifiers.
        /// </summary>
        /// <param name="other">
        /// the set to test for
        /// </param>
        /// <returns>
        /// true in case of min. one matches
        /// </returns>
        public bool MatchesAny(ProfileIdentifierDictionary other)
        {
            return other.Any(identifier => this.GetProfileId(identifier.Key) == identifier.Value);
        }

        /// <summary>
        /// Sets a specific profile id.
        /// </summary>
        /// <param name="type">
        /// the profile type to set 
        /// </param>
        /// <param name="newValue">
        /// the new profile id 
        /// </param>
        /// <returns>
        /// The new profile id. 
        /// </returns>
        public ProfileIdInformation SetProfileId(ProfileIdentifierType type, ProfileIdInformation newValue)
        {
            return this.SetProfileId(type, newValue, false);
        }

        /// <summary>
        /// Sets a specific profile id.
        /// </summary>
        /// <param name="type">
        /// the profile type to set 
        /// </param>
        /// <param name="newValue">
        /// the new profile id 
        /// </param>
        /// <param name="doNotOverwriteExisting">
        /// If set to true, an existing value will not be overwritten. 
        /// </param>
        /// <returns>
        /// The profile id that does exist in the dictionary after this operation. 
        /// </returns>
        public ProfileIdInformation SetProfileId(
            ProfileIdentifierType type, ProfileIdInformation newValue, bool doNotOverwriteExisting)
        {
            if (this.ContainsKey(type))
            {
                if (doNotOverwriteExisting)
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
        /// Overrides the ToString implementation by returning a meaningful string
        /// </summary>
        /// <returns>
        /// a string containing the ids 
        /// </returns>
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

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new value for the given string.
        /// </summary>
        /// <param name="value">
        /// The value to create a new value of type <see cref="ProfileIdInformation"/>. 
        /// </param>
        /// <returns>
        /// The new value 
        /// </returns>
        protected override ProfileIdInformation CreateNewValueItem(string value)
        {
            return value;
        }

        /// <summary>
        /// To keep compatibility while deserializing information, some text is translated.
        /// </summary>
        /// <param name="keyName">
        /// The key name to be translated. 
        /// </param>
        /// <returns>
        /// The translated string. 
        /// </returns>
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

        #endregion
    }
}