// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceDescription.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the SourceDescription type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsAccess
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using GenericHelpers.Entities;

    using SyncBase.DetailData;
    using Sem.Sync.SyncBase;

    /// <summary>
    /// Defines a database source
    /// </summary>
    public class SourceDescription
    {
        /// <summary>
        /// Gets or sets the path to the Access database file or an OLEDB connection string.
        /// </summary>
        public string DatabasePath { get; set; }

        /// <summary>
        /// Gets or sets the table to use.
        /// </summary>
        public string MainTable { get; set; }

        /// <summary>
        /// Gets or sets the column definitions.
        /// </summary>
        public List<ColumnDefinition> ColumnDefinitions { get; set; }

        /// <summary>
        /// Generates a standard sample for looking up the correct XML syntax
        /// </summary>
        /// <returns> a sample mapping definition file </returns>
        public static SourceDescription GetDefaultSourceDescription()
        {
            var returnValue = new SourceDescription
                {
                    DatabasePath =
                        Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "AccessDatabaseSample.mdb"),
                    MainTable = "ContactInformation",
                    ColumnDefinitions =
                        new List<ColumnDefinition>
                            {
                                new ColumnDefinition
                                    {
                                        Selector = "PersonalProfileIdentifiers.MicrosoftAccessId",
                                        Title = "Id",
                                        IsPrimaryKey = true,
                                        IsAutoValue = true,
                                    },
                                new ColumnDefinition { Selector = "Name.FirstName", Title = "Vorname" },
                                new ColumnDefinition { Selector = "Name.LastName", Title = "Nachname" },
                                new ColumnDefinition { Selector = "DateOfBirth", Title = "Geburtstag" },
                                new ColumnDefinition
                                    {
                                        Selector = "PersonalProfileIdentifiers.ActiveDirectoryId",
                                        Title = "Some-ID",
                                        IsLookupValue = true,
                                    },
                                new ColumnDefinition
                                    {
                                        Selector = "BusinessAddressPrimary.Phone.DenormalizedPhoneNumber",
                                        Title = "Telefonnummer",
                                    },
                                new ColumnDefinition { Selector = "BusinessEmailPrimary", Title = "Mailadresse" },
                                new ColumnDefinition { Selector = "BusinessAddressPrimary.Room", Title = "Raum_local" },
                                new ColumnDefinition
                                    {
                                        Selector = "PersonGender", 
                                        Title = "Geschlecht",
                                        TransformationToDatabase = 
                                        (ColumnDefinition, value) => 
                                            ((Gender)value) == Gender.Male 
                                            ? "'m'" 
                                            : ((Gender)value) == Gender.Female ? "'f'" : "NULL",
                                        TransformationFromDatabase = 
                                        (ColumnDefinition, value) => 
                                            ((string)value) == "m" 
                                            ? Gender.Male 
                                            : ((string)value) == "f" ? Gender.Female : Gender.Unspecified,
                                    },
                                new TableLink 
                                { 
                                    TableName = "Firma",
                                    JoinBy = new List<KeyValuePair>{ new KeyValuePair {Key = "Firma", Value = "Id"}},                                    

                                    ColumnDefinitions = new List<ColumnDefinition>
                                    {
                                        new ColumnDefinition {Title = "Firmenname", Selector = "BusinessCompanyName",}                                        
                                    }
                                },
                                new TableLink 
                                { 
                                    TableName = "Rolle",
                                    JoinBy = new List<KeyValuePair>{ new KeyValuePair {Key = "Rolle", Value = "RolleId"}},                                    

                                    ColumnDefinitions = new List<ColumnDefinition>
                                    {
                                        new ColumnDefinition {Title = "Rollenname", Selector = "BusinessPosition",}                                        
                                    }
                                },
                            }
                };

            return returnValue;
        }

        /// <summary>
        /// Determines the PK name
        /// </summary>
        /// <returns> the PK name of this definition </returns>
        public string GetPrimaryKeyName()
        {
            return (from x in this.ColumnDefinitions where x.IsPrimaryKey select x.Title).FirstOrDefault();
        }
    }
}
