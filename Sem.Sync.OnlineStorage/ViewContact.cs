using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Sem.Sync.OnlineStorage
{
    [DataContract(Name = "ViewContact")]
    public class ViewContact
    {
        [DataMember(Name = "FullName")]
        public string FullName { get; set; }

        [DataMember(Name = "Picture")]
        public Byte[] Picture { get; set; }

        [DataMember(Name = "Street")]
        public string Street { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }
    }
}
