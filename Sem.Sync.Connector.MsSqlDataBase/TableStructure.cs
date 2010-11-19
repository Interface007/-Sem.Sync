// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableStructure.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The table structure.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsSqlDatabase
{
    using System.Collections.Generic;

    /// <summary>
    /// The table structure.
    /// </summary>
    public class TableStructure
    {
        /// <summary>
        /// Gets or sets the DatabaseTable name.
        /// </summary>
        public string DatabaseTable { get; set; }

        /// <summary>
        /// Gets or sets the PKField for this table structure.
        /// </summary>
        public List<string> PKField { get; set; }

        /// <summary>
        /// Gets a default table structure (one table with one PK-Field).
        /// </summary>
        /// <returns>Returns a list of table structures with one entry.</returns>
        public static List<TableStructure> GetDefault()
        {
            return new List<TableStructure>
                {
                    new TableStructure 
                        {
                            DatabaseTable = "[dbo].[Contact]", 
                            PKField = new List<string> { "[ContactID]" }
                        }
                };
        }
    }
}