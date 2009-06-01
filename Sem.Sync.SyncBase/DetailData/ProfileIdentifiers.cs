//-----------------------------------------------------------------------
// <copyright file="ProfileIdentifiers.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    public enum ProfileIdentifierType
    {
        Default = 0,
        XingProfileId,
        FacebookProfileId,
        ActiveDirectoryId,
    }

    public class ProfileIdentifiers
    {
        public string XingProfileId { get; set; }
        public string FacebookProfileId { get; set; }
        public string ActiveDirectoryId { get; set; }

        public ProfileIdentifiers() { }

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
            }
        }

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

            }
            return string.Empty;
        }

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

            }
        }

        public bool MatchesAny(ProfileIdentifiers other)
        {
            return (other == null)
                ? false
                : (!string.IsNullOrEmpty(this.XingProfileId) && this.XingProfileId == other.XingProfileId) ||
                  (!string.IsNullOrEmpty(this.ActiveDirectoryId) && this.ActiveDirectoryId == other.ActiveDirectoryId) ||
                  (!string.IsNullOrEmpty(this.FacebookProfileId) && this.FacebookProfileId == other.FacebookProfileId);
        }
    }
}