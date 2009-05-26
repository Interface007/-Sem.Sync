namespace Sem.Sync.SyncBase.DetailData
{
    public class AddressDetail
    {
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string PostalCode { get; set; }
        public string CityName { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string Room { get; set; }
        public string StreetNumberExtension { get; set; }
        public PhoneNumber Phone { get; set; }

        public override string ToString()
        {
            var result = StreetName ?? "";
            result += " " + StreetNumber;
            result += (StreetNumberExtension ?? "") + " / ";
            result += (PostalCode ?? "") + " ";
            result += CityName ?? "";
            result += " (" + (StateName ?? "") +")";
            result += CountryName ?? "";
            result += " Phone: " + ((Phone == null) ? "" : Phone.ToString());

            return (result == " 0 /   () Phone: ") ? null : result;
        }
    }
}