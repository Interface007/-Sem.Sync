namespace Sem.Sync.OnlineStorage
{
    using System.Linq;

    using Connector.Filesystem;

    using SyncBase.DetailData;
    using SyncBase.Helpers;
    
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
