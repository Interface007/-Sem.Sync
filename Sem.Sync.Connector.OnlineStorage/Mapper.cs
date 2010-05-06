using System;
using System.Collections.Generic;

namespace Sem.Sync.Connector.OnlineStorage
{
    internal static class Mapper
    {
        internal static SyncBase.StdElement ToStdElementBase(this Sem.Sync.Connector.OnlineStorage.OnlineStorage.StdContact contact)
        {
            throw new NotImplementedException();
        }

        internal static SyncBase.StdElement ToStdElementBase(this Sem.Sync.Connector.OnlineStorage.ContactService2.StdContact contact)
        {
            throw new NotImplementedException();
        }

        internal static SyncBase.StdElement ToStdElementBase(this Sem.Sync.Connector.OnlineStorage.Cloud.StdContact contact)
        {
            throw new NotImplementedException();
        }

        internal static List<Sem.Sync.Connector.OnlineStorage.OnlineStorage.StdContact> ToStdContactsService(this List<Sem.Sync.SyncBase.StdElement> contact)
        {
            throw new NotImplementedException();
        }

        internal static List<ContactService2.StdContact> ToStdContactsService2(this List<Sem.Sync.SyncBase.StdElement> contact)
        {
            throw new NotImplementedException();
        }

        internal static List<Sem.Sync.Connector.OnlineStorage.Cloud.StdContact> ToStdContactsCloud(this List<Sem.Sync.SyncBase.StdElement> contact)
        {
            throw new NotImplementedException();
        }

        internal static Sem.Sync.Connector.OnlineStorage.OnlineStorage.StdContact ToStdElementService(this SyncBase.StdElement contact)
        {
            throw new NotImplementedException();
        }
    }
}
