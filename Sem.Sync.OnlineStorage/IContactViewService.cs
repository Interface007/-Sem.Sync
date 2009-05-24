using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Sem.Sync.OnlineStorage
{
    // NOTE: If you change the interface name "IContactViewService" here, you must also update the reference to "IContactViewService" in Web.config.
    [ServiceContract]
    public interface IContactViewService
    {
        [OperationContract]
        ViewContact[] GetAll(string clientFolderName);
    }
}
