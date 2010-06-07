// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigurable.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IConfigurable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Interfaces
{
    /// <summary>
    /// Provides the interface to edit client configuration data.
    /// </summary>
    public interface IConfigurable
    {
        /// <summary>
        /// Opens a client side dialog to edit configuration data. This method must 
        /// block the execution until the editing is done - that is not possible
        /// for web interfaces, so we will need a more generic interface to support
        /// that type of applications, too.
        /// </summary>
        /// <param name="configuration"> The configuration data to be edited. </param>
        /// <returns> The edited configuration data (might be same as before). </returns>
        string ShowConfigurationDialog(string configuration);
    }
}
