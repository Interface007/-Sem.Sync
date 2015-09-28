// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableLink.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines a link between tables in the database schema
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a link between tables in the database schema
    /// </summary>
    public class TableLink : ColumnDefinition
    {
        #region Properties

        /// <summary>
        ///   Gets or sets ColumnDefinitions.
        /// </summary>
        public List<ColumnDefinition> ColumnDefinitions { get; set; }

        /// <summary>
        ///   Gets or sets a list of columns to perform the join.
        /// </summary>
        public List<KeyValuePair> JoinBy { get; set; }

        /// <summary>
        ///   Gets or sets the name of the linked table.
        /// </summary>
        public string TableName { get; set; }

        #endregion
    }
}