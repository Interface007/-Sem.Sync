// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlContactsByCompanyConfigurationData.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DgmlContactsByCompanyConfigurationData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic
{
    /// <summary>
    /// Theis defines the data structure that can be modified with the 
    /// data editor provided by <see cref="DgmlContactsByCompanyConfigurationEditor"/>.
    /// </summary>
    public class DgmlContactsByCompanyConfigurationData
    {
        /// <summary>
        /// Gets or sets the property path to the grouping property - might be empty.
        /// </summary>
        public string GroupingPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the destination file name and path for the DGML path.
        /// </summary>
        public string DestinationPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DgmlContactsByCompanyConfigurationData"/> class.
        /// </summary>
        public DgmlContactsByCompanyConfigurationData()
        {
            this.GroupingPropertyName = "BusinessCompanyName";
        }
    }
}
