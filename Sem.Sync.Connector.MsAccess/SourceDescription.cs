namespace Sem.Sync.Connector.MsAccess
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class SourceDescription
    {
        public string DatabasePath { get; set; }
        public string MainTable { get; set; }
        public List<Mapping> Mappings { get; set; }

        public static SourceDescription GetDefaultSourceDescription()
        {
            var returnValue = new SourceDescription
                {
                    DatabasePath =
                        Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "AccessDatabaseSample.mdb"),
                    MainTable = "ContactInformation",
                    Mappings =
                        new List<Mapping>
                            {
                                new Mapping
                                    {
                                        PropertyPath = "PersonalProfileIdentifiers.MicrosoftAccessId",
                                        FieldName = "Id",
                                        IsPrimaryKey = true,
                                        IsAutoValue = true,
                                    },
                                new Mapping { PropertyPath = "Name.FirstName", FieldName = "Vorname" },
                                new Mapping { PropertyPath = "Name.LastName", FieldName = "Nachname" },
                                new Mapping { PropertyPath = "DateOfBirth", FieldName = "Geburtstag" },
                                new Mapping
                                    {
                                        PropertyPath = "PersonalProfileIdentifiers.ActiveDirectoryId",
                                        FieldName = "ComSi-ID",
                                        IsLookupValue = true,
                                    },
                                new Mapping
                                    {
                                        PropertyPath = "BusinessAddressPrimary.Phone.DenormalizedPhoneNumber",
                                        FieldName = "Telefonnummer",
                                    },
                                new Mapping { PropertyPath = "BusinessEmailPrimary", FieldName = "Mailadresse" },
                                new Mapping { PropertyPath = "BusinessAddressPrimary.Room", FieldName = "Raum_local" }
                            }
                };

            return returnValue;
        }

        public string GetPrimaryKeyName()
        {
            return (from x in this.Mappings where x.IsPrimaryKey select x.FieldName).FirstOrDefault();
        }
    }
}
