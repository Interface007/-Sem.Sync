using System.Configuration;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Sem.Sync.SyncBase.Helpers
{
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
            var value = ConfigurationManager.AppSettings["Sem.Sync.SyncBase-VersionCheck"];
            var doCheck = true;
            bool.TryParse(value, out doCheck);
            if (!doCheck) return true;
            
            var myVersion = new VersionCheck();

            var formatter = new XmlSerializer(typeof(VersionCheck));
            var reader = new StringReader((new HttpHelper(VersionBaseUrl, false)).GetContent(VersionXmlUrl, "[NOCACHE]"));
            var serverVersion = (VersionCheck)formatter.Deserialize(reader);

            return 
                serverVersion.Major == myVersion.Major &&
                serverVersion.Minor == myVersion.Minor &&
                serverVersion.MajorRevision == myVersion.MajorRevision &&
                serverVersion.MinorRevision == myVersion.MinorRevision &&
                serverVersion.Build == myVersion.Build;
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
