// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProfileIdInformation.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class describes a profile link. It contains a profile Id which identifies the
//   person in a specific data context (e.g. the user name inside an active directory domain).
//   It also contains a resource locator which is additional information to access the
//   contact information inside the target system.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// This class describes a profile link. It contains a profile Id which identifies the 
    /// person in a specific data context (e.g. the user name inside an active directory domain).
    /// It also contains a resource locator which is additional information to access the
    /// contact information inside the target system.
    /// </summary>
    public class ProfileIdInformation : IComparable<ProfileIdInformation>
    {
        public static implicit operator ProfileIdInformation(string value)
        {
            return new ProfileIdInformation { Id = value };
        }

        public static explicit operator string(ProfileIdInformation value)
        {
            return value == null ? null : value.Id;
        }

        public static bool operator ==(ProfileIdInformation value1, ProfileIdInformation value2)
        {
            return value1.CompareTo(value2) == 0;
        }

        public static bool operator !=(ProfileIdInformation value1, ProfileIdInformation value2)
        {
            return value1.CompareTo(value2) != 0;
        }

        public static bool operator <(ProfileIdInformation value1, ProfileIdInformation value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        public static bool operator >(ProfileIdInformation value1, ProfileIdInformation value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        [XmlText]
        public string Id { get; set; }

        [XmlAttribute]
        public string ResourceLocator { get; set; }

        #region IComparable Members

        public int CompareTo(ProfileIdInformation other)
        {
            if (other == null)
            {
                return -1;
            }

            return string.Compare(this.Id, other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return this.CompareTo(obj as ProfileIdInformation) == 0;
        }

        public override int GetHashCode()
        {
            return -1;
        }

        #endregion
    }
}
