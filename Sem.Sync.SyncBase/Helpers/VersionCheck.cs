//-----------------------------------------------------------------------
// <copyright file="VersionCheck.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Helpers
{
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;
    
    public class VersionCheck
    {
        private const string VersionBaseUrl = "http://svenerikmatzen.info";
        private const string VersionXmlUrl = "/Content/Portals/0/sem.sync.version.xml";

        public int Build { get; set; }
        public int Major { get; set; }
        public short MajorRevision { get; set; }
        public int Minor { get; set; }
        public short MinorRevision { get; set; }
        public int Revision { get; set; }

        public static bool Check()
        {
            try
            {
                var value = ConfigurationManager.AppSettings["Sem.Sync.SyncBase-VersionCheck"];
                bool doCheck;
                if (bool.TryParse(value, out doCheck))
                    if (!doCheck) return true;

                var myVersion = new VersionCheck();

                var formatter = new XmlSerializer(typeof(VersionCheck));
                var reader = new StringReader((new HttpHelper(VersionBaseUrl, false)).GetContent(VersionXmlUrl, "[NOCACHE]"));
                var serverVersion = (VersionCheck)formatter.Deserialize(reader);

                return
                    serverVersion.Major <= myVersion.Major &&
                    serverVersion.Minor <= myVersion.Minor &&
                    serverVersion.MajorRevision <= myVersion.MajorRevision &&
                    serverVersion.MinorRevision <= myVersion.MinorRevision &&
                    serverVersion.Build <= myVersion.Build;
            }
            catch // catch simply all - if there's a problem, we will check next time
            {
                return true;
            }
        }

        public VersionCheck()
        {
            var myVersion = Assembly.GetExecutingAssembly().GetName().Version;

            this.Build = myVersion.Build;
            this.Major = myVersion.Major;
            this.MajorRevision = myVersion.MajorRevision;
            this.Minor = myVersion.Minor;
            this.MinorRevision = myVersion.MinorRevision;
            this.Revision = myVersion.Revision;
        }
    }
}
