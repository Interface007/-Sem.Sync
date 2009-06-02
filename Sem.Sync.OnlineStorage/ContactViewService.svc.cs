using Sem.Sync.SyncBase.DetailData;

namespace Sem.Sync.OnlineStorage
{

    using System.Linq;
    using SyncBase.Helpers;
    using FilesystemConnector;
    using System.ServiceModel;
    using Sem.Sync.SyncBase;

    public class ContactViewService : IContactViewService
    {
        private string StoragePath = (new Properties.Settings()).StoragePath; // "C:\\ContactsServerData\\Contacts.xml";
        public ViewContact[] GetAll(string clientFolderName)
        {
            ViewContact[] stdContacts;

            stdContacts = 
                (from x in new ContactClient().GetAll(StoragePath).ToContacts()
                 select new ViewContact
                            {
                                FullName = x.GetFullName(),
                                City = (x.PersonalAddressPrimary ?? x.BusinessAddressPrimary ?? new AddressDetail{CityName = ""}).CityName,
                                Street = (x.PersonalAddressPrimary ?? x.BusinessAddressPrimary ?? new AddressDetail { StreetName = "" }).StreetName,
                                Picture = x.PictureData
                            }
                ).ToArray();
            
            return stdContacts;
        }
    }
}
