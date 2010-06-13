using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System.Xml.Serialization;

    [XmlType(TypeName = "number")]
    public class Telephony : List<PhoneNumber>
    {
    }
}
