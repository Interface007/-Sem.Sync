// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClientCsv.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
// This client implementation does write to a CSV file. This implementation is actually very simple with 
// fixed fields. In the future there may be an implementation with configurable fields and column headers.
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

    /// <summary>
    /// This client implementation does write to a CSV file. This implementation is actually very simple with 
    /// fixed fields. In the future there may be an implementation with configurable fields and column headers.
    /// </summary>
    public class ContactClientCsv : StdClient
    {
        /// <summary>
        /// Gets the user readable name of the client implementation called "Comma Seperated Values connector".
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Comma Seperated Values connector";
            }
        }

        /// <summary>
        /// This is a write only client, so this method will simply return the <paramref name="result"/>.
        /// </summary>
        /// <param name="clientFolderName">this parameter is not used</param>
        /// <param name="result">this list will be returned by the method</param>
        /// <returns>the list from parameter <paramref name="result"/></returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            return result;
        }

        /// <summary>
        /// Write method for full list of elements. This will write the properties of the <see cref="StdElement"/>
        /// to the file specified by the parameter <paramref name="clientFolderName"/>.
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">The path and file name to where the elements should be written.</param>
        /// <param name="skipIfExisting">this parameter is ignored in this client implementation</param>
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

                headerLine.Append("BusinessCompanyName;");
                headerLine.Append("BusinessEmailPrimary;");
                headerLine.Append("BusinessPhoneMobile");

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

                        line.Append(element.BusinessCompanyName + ";");
                        line.Append(element.BusinessEmailPrimary + ";");
                        line.Append(element.BusinessPhoneMobile + ";");
    
                        line.Append(element.AdditionalTextData + ";");

                        line.Append(element.InternalSyncData.DateOfCreation.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ";");
                        line.Append(element.InternalSyncData.DateOfLastChange.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                        file.WriteLine(line.ToString().Replace("\n", " ").Replace("\r", " "));
                    }
                }
            }
        }
    }
}
