// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StubStorage.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This is a simple stub for "some" cloud storage. It does provide some
//   test data (different contacts), but does not persist any data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.CloudStorageConnector
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    using SyncBase;
    using SyncBase.Helpers;

    using Test.DataGenerator;

    /// <summary>
    /// This is a simple stub for "some" cloud storage. It does provide some
    /// test data (different contacts), but does not persist any data.
    /// </summary>
    public class StubStorage : StdClient
    {
        /// <summary>
        /// This is the formatter instance for serializing the list of contacts.
        /// </summary>
        private static readonly XmlSerializer ContactListFormatter = new XmlSerializer(typeof(List<StdContact>));
        
        public override string FriendlyClientName
        {
            get
            {
                return "Azure storage stub";
            }
        }

        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var resultXml = new StringBuilder(102400);
            resultXml.AppendLine("<?xml version=\"1.0\"?>");
            resultXml.AppendLine("<ArrayOfStdContact xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

            Contacts.AddContactWithoutPicture(resultXml);
            Contacts.AddContactWithPicture(resultXml);
            Contacts.AddContactWithNulls(resultXml);

            resultXml.Append("</ArrayOfStdContact>");
            var textStream = new MemoryStream(Encoding.UTF8.GetBytes(result.ToString()));

            result = ((List<StdContact>)ContactListFormatter.Deserialize(textStream)).ToStdElement();
            CleanUpEntities(result);
            
            return result;
        }

        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new System.NotImplementedException();
        }
    }
}