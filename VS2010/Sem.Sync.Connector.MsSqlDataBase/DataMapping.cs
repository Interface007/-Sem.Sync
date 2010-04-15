// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataMapping.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DataMapping type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsSqlDatabase
{
    using System.Collections.Generic;

    /// <summary>
    /// Mapping class - might be replaced with a generic one in a base library
    /// </summary>
    public class DataMapping
    {
        public string PathToProperty { get; set; }
        public string DatabaseTable { get; set; }
        public string DatabaseField { get; set; }
        public string ReferenceToDatabaseTable { get; set; }
    }

    public class TableStructure
    {
        public string DatabaseTable { get; set; }
        public List<string> PKField { get; set; }

        public static List<TableStructure> GetDefault()
        {
            return new List<TableStructure>
                       {
                           new TableStructure
                               {
                                   DatabaseTable = "[dbo].[Contact]",
                                   PKField = new List<string> 
                                   {
                                       "[ContactID]"
                                   }
                               }
                       };
        }
    }
}
