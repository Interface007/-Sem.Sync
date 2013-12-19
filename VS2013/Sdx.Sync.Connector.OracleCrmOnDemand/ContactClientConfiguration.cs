// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClientConfiguration.cs" company="SDX-AG">
//   (c) 2010 SDX-AG
// </copyright>
// <summary>
//   Defines the ContactClientConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System.Windows.Forms;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Implements the <see cref="IConfigurable"/> interface in order to show a dialog 
    /// for editing the configuration data.
    /// </summary>
    public class ContactClientConfiguration : IConfigurable
    {
        /// <summary>
        /// Shows the editor for configuration data to the user.
        /// </summary>
        /// <param name="configuration"> The serialized configuration data. </param>
        /// <returns> The edited serialized configuration data. </returns>
        public string ShowConfigurationDialog(string configuration)
        {
            var editorData =
                Tools.LoadFromString<ContactClientConfigurationData>(configuration)
                ?? new ContactClientConfigurationData();

            var editor = new ContactClientConfigurationEditor();

            return
                editor.ShowDialog(editorData) == DialogResult.OK
                ? Tools.SaveToString(editorData)
                : configuration;
        }
    }
}
