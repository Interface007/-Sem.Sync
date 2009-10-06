namespace Sem.Sync.Connector.MsAccess
{
    using System.Collections.Generic;

    public class SourceDescription
    {
        public string DatabasePath { get; set; }
        public string MainTable { get; set; }
        public List<Mapping> Mappings { get; set; }

        public SourceDescription()
        {
            this.DatabasePath = "C:\\Documents and Settings\\EH2MAIK\\Desktop\\Copy of Project_Employees_Eva_Tom.mdb";
            this.MainTable = "Commerzbank_Employee_Tracking_Eva_Tom";
            this.Mappings = new List<Mapping>
                {
                    new Mapping
                    {
                        PropertyPath = "PersonalProfileIdentifiers.MicrosoftAccessId",
                        TableName = "Commerzbank_Employee_Tracking_Eva_Tom",
                        FieldName = "Id",
                        IsPrimaryKey = true,
                        IsAutoValue = true,
                    },
                    new Mapping
                    {
                        PropertyPath = "Name.FirstName",
                        TableName = "Commerzbank_Employee_Tracking_Eva_Tom",
                        FieldName = "Vorname"
                    },
                    new Mapping
                    {
                        PropertyPath = "Name.LastName",
                        TableName = "Commerzbank_Employee_Tracking_Eva_Tom",
                        FieldName = "Nachname"
                    },
                    new Mapping
                    {
                        PropertyPath = "DateOfBirth",
                        TableName = "Commerzbank_Employee_Tracking_Eva_Tom",
                        FieldName = "Geburtstag"
                    },
                    new Mapping
                    {
                        PropertyPath = "PersonalProfileIdentifiers.ActiveDirectoryId",
                        TableName = "Commerzbank_Employee_Tracking_Eva_Tom",
                        FieldName = "ComSi-ID"
                    },
                    new Mapping
                    {
                        PropertyPath = "BusinessAddressPrimary.Phone.DenormalizedPhoneNumber",
                        TableName = "Commerzbank_Employee_Tracking_Eva_Tom",
                        FieldName = "Telefonnummer"
                    },
                    new Mapping
                    {
                        PropertyPath = "BusinessEmailPrimary",
                        TableName = "Commerzbank_Employee_Tracking_Eva_Tom",
                        FieldName = "Mailadresse"
                    },
                    new Mapping
                    {
                        PropertyPath = "BusinessAddressPrimary.Room",
                        TableName = "Commerzbank_Employee_Tracking_Eva_Tom",
                        FieldName = "Raum_local"
                    }
                };
        }
    }
}
