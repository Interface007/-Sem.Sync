// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlContactsByCompanyConfiguration.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DgmlContactsByCompanyConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic
{
    using System.Windows.Forms;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Provides the interface to edit client configuration data.
    /// </summary>
    public class DgmlContactsByCompanyConfiguration : IConfigurable
    {
        /// <summary>
        /// Opens a client side dialog to edit configuration data.
        /// </summary>
        /// <param name="configuration"> The configuration data to be edited. </param>
        /// <returns> The edited configuration data (might be same as before). </returns>
        public string ShowConfigurationDialog(string configuration)
        {
            var editorData = 
                Tools.LoadFromString<DgmlContactsByCompanyConfigurationData>(configuration)
                ?? new DgmlContactsByCompanyConfigurationData();

            var editor = new DgmlContactsByCompanyConfigurationEditor();
            
            return
                editor.ShowDialog(editorData) == DialogResult.OK
                ? Tools.SaveToString(editorData) 
                : configuration;
        }
    }
}
