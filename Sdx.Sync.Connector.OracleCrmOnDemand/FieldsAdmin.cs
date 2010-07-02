// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldsAdmin.cs" company="SDX-AG">
//   (c) 2010 SDX-AG
// </copyright>
// <summary>
//   Defines the FieldsAdmin type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System.Collections.Generic;

    using Sdx.Sync.Connector.OracleCrmOnDemand.FieldManagementSR;

    /// <summary>
    /// Implements administrative access to fields mapping
    /// </summary>
    public static class FieldsAdmin
    {
        /// <summary>
        /// Queries the fields mapping of custom fields to user defined names
        /// </summary>
        /// <param name="client"> The query client. </param>
        /// <returns> a dictionary with the crm field name (key) to the custom name (value) </returns>
        public static IDictionary<string, string> FieldMapping(FieldManagementServiceClient client)
        {
            // todo: these mappings need to be migrated to settings in the users folder 
            // - they depend on the oracle costumers settings
            var result = new Dictionary<string, string>
                {
                    { "Contact.CustomBoolean20", "ist aktiv" },
                    { "Contact.CustomBoolean2", "Infoletter" },
                    { "Contact.CustomBoolean3", "Infoletter abbestellt" },
                    { "Contact.CustomBoolean4", "Direktmail" },
                    { "Contact.CustomBoolean5", "Direktmail abbestellt" },
                    { "Contact.CustomBoolean1", "X-Card" },
                    { "Contact.CustomBoolean25", "X-Card 09" },
                    { "Contact.CustomBoolean26", "X-Gift 09" }
                };

            // todo: we need to query the fields mapping from the oracle crm service or at least make it configurable
            ////var input = new FieldManagementReadAll_Input
            ////                {
            ////                };

            ////var x = client.FieldManagementReadAll(input);

            return result;
        }
    }
}
