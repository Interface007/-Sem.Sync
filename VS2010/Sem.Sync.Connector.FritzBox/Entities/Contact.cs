namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType(TypeName = "contact")]
    public class Contact
    {
        [XmlElement(ElementName = "category")]
        public int Category { get; set; }

        [XmlElement(ElementName = "person")]
        public Person Person { get; set; }

        [XmlElement(ElementName = "telephony")]
        public List<PhoneNumber> Telephony { get; set; }

        [XmlElement(ElementName = "services")]
        public string Services { get; set; }

        [XmlElement(ElementName = "setup")]
        public string setup { get; set; }
    }
}
