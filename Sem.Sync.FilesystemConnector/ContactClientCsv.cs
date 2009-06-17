// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClientCsv.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactClientCsv type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.FilesystemConnector
{
    #region usings

    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    #endregion usings

    public class ContactClientCsv : StdClient
    {
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            return result;
        }

        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            using (var file = new StreamWriter(clientFolderName))
            {
                var headerLine = new StringBuilder();
                headerLine.Append("GetFullName;");

                headerLine.Append("ActiveDirectoryId;");

                headerLine.Append("Gender;");

                headerLine.Append("FirstName;");
                headerLine.Append("MiddleName;");
                headerLine.Append("LastName;");

                headerLine.Append("BusinessDepartment;");

                headerLine.Append("BusinessDepartmentCountryName;");
                headerLine.Append("BusinessDepartmentCityName;");
                headerLine.Append("BusinessDepartmentPostalCode;");
                headerLine.Append("BusinessDepartmentStreetName;");
                headerLine.Append("BusinessDepartmentRoom;");
                headerLine.Append("BusinessDepartmentPhone;");

                headerLine.Append("BusinessEmailPrimary;");

                headerLine.Append("AdditionalTextData;");

                headerLine.Append("DateOfCreation;");
                headerLine.Append("DateOfLastChange");

                file.WriteLine(headerLine);

                foreach (StdContact element in elements)
                {
                    SyncTools.ClearNulls(element, typeof(StdContact));
                    if (element.Name != null)
                    {
                        var line = new StringBuilder();
                        line.Append(element.GetFullName() + ";");
                        
                        line.Append(element.PersonalProfileIdentifiers.GetProfileId(ProfileIdentifierType.ActiveDirectoryId) + ";");

                        line.Append(element.PersonGender + ";");
                        line.Append(element.Name.FirstName + ";");
                        line.Append(element.Name.MiddleName + ";");
                        line.Append(element.Name.LastName + ";");

                        line.Append(element.BusinessDepartment + ";");

                        if (element.BusinessAddressPrimary != null)
                        {
                            line.Append(element.BusinessAddressPrimary.CountryName + ";");
                            line.Append(element.BusinessAddressPrimary.CityName + ";");
                            line.Append(element.BusinessAddressPrimary.PostalCode + ";");
                            line.Append(element.BusinessAddressPrimary.StreetName + ";");
                            line.Append(element.BusinessAddressPrimary.Room + ";");
                            line.Append(element.BusinessAddressPrimary.Phone + ";");
                        }
                        else
                        {
                            line.Append(";;;;;;");                            
                        }

                        line.Append(element.BusinessEmailPrimary + ";");

                        line.Append(element.AdditionalTextData + ";");

                        line.Append(element.InternalSyncData.DateOfCreation.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ";");
                        line.Append(element.InternalSyncData.DateOfLastChange.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                        file.WriteLine(line.ToString().Replace("\n", " ").Replace("\r", " "));
                    }
                }
            }
        }

        public override string FriendlyClientName
        {
            get
            {
                return "Comma Seperated Values connector";
            }
        }
    }
}
