namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.Xml.Serialization;

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

        #endregion
    }
}
