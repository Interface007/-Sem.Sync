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
        /// <summary>
        /// Gets or sets the profile Id, which must be unique for the <see cref="ResourceLocator"/>.
        /// </summary>
        [XmlText]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the ResourceLocator - for social networks this may be an empty string or null if the
        /// assumption is valid that all ids are unique for the connector.
        /// </summary>
        [XmlAttribute]
        public string ResourceLocator { get; set; }

        /// <summary>
        /// Assigns a <see cref="ProfileIdInformation"/> directly from a string.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> a new profile id information from a string representation </returns>
        public static implicit operator ProfileIdInformation(string value)
        {
            var parts = value.Split(new[] { "[@]" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                return new ProfileIdInformation { Id = value };
            }

            return new ProfileIdInformation
                {
                    Id = value, 
                    ResourceLocator = parts[1]
                };
        }

        /// <summary>
        /// Allows direct assignment from <see cref="ProfileIdInformation"/> to string.
        /// </summary>
        /// <param name="value"> The <see cref="ProfileIdInformation"/> to be converted to a string. </param>
        /// <returns> a string representing the <see cref="ProfileIdInformation"/> </returns>
        public static implicit operator string(ProfileIdInformation value)
        {
            return value as object == null ? null : value.ToString();
        }

        /// <summary>
        /// Compares two <see cref="ProfileIdInformation"/> instances.
        /// </summary>
        /// <param name="value1"> The value 1. </param>
        /// <param name="value2"> The value 2. </param>
        /// <returns> true if both <see cref="ProfileIdInformation"/> instances are equal</returns>
        public static bool operator ==(ProfileIdInformation value1, ProfileIdInformation value2)
        {
            if (value1 as object == null || value2 as object == null)
            {
                return false;
            }
            
            return value1.CompareTo(value2) == 0;
        }

        /// <summary>
        /// Compares two <see cref="ProfileIdInformation"/> instances.
        /// </summary>
        /// <param name="value1"> The value 1. </param>
        /// <param name="value2"> The value 2. </param>
        /// <returns> true if both <see cref="ProfileIdInformation"/> instances are NOT equal</returns>
        public static bool operator !=(ProfileIdInformation value1, ProfileIdInformation value2)
        {
            return value1.CompareTo(value2) != 0;
        }

        /// <summary>
        /// Compares two <see cref="ProfileIdInformation"/> instances.
        /// </summary>
        /// <param name="value1"> The value 1. </param>
        /// <param name="value2"> The value 2. </param>
        /// <returns> true if the first <see cref="ProfileIdInformation"/> instances is lower than the second instance</returns>
        public static bool operator <(ProfileIdInformation value1, ProfileIdInformation value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        /// <summary>
        /// Compares two <see cref="ProfileIdInformation"/> instances.
        /// </summary>
        /// <param name="value1"> The value 1. </param>
        /// <param name="value2"> The value 2. </param>
        /// <returns> true if the first <see cref="ProfileIdInformation"/> instances is higher than the second instance</returns>
        public static bool operator >(ProfileIdInformation value1, ProfileIdInformation value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        /// <summary>
        /// Implements the <see cref="IComparable"/> interface.
        /// </summary>
        /// <param name="other"> The other instance to compare to. </param>
        /// <returns> A value indicating how this instance compares to the other - see <see cref="IComparable"/> for more details.</returns>
        public int CompareTo(ProfileIdInformation other)
        {
            if (other as object == null)
            {
                return -1;
            }

            return string.Compare(this.Id, other.Id, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Implements the <see cref="IComparable"/> interface.
        /// </summary>
        /// <param name="other"> The other instance to compare to. </param>
        /// <returns> A value indicating how this instance compares to the other - see <see cref="IComparable"/> for more details.</returns>
        public override bool Equals(object other)
        {
            ProfileIdInformation otherInstance;
            if (other.GetType().Name == "String")
            {
                otherInstance = (string)other;
            }
            else
            {
                otherInstance = other as ProfileIdInformation;
            }

            return this.CompareTo(otherInstance) == 0;
        }

        /// <summary>
        /// Implements the <see cref="IComparable"/> interface.
        /// </summary>
        /// <returns> A value identifying this object. This value is NOT guaranteed to be unique (currently it's always -1)</returns>
        public override int GetHashCode()
        {
            return -1;
        }

        /// <summary>
        /// Overrides the ToString() method to return the Id and the resource locator.
        /// This method uses the "[@]" sequence as a seperator if there is a <see cref="ResourceLocator"/>
        /// </summary>
        /// <returns>
        /// A string representing a unique identifier for a contact in a specific type of source
        /// </returns>
        public override string ToString()
        {
            return this.Id + (string.IsNullOrEmpty(this.ResourceLocator) ? string.Empty : "[@]" + this.ResourceLocator);
        }
    }
}
