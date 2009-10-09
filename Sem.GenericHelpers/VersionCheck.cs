//-----------------------------------------------------------------------
// <copyright file="VersionCheck.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.GenericHelpers
{
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;

    using Interfaces;

    /// <summary>
    /// Checks the version of a library.
    /// </summary>
    public class VersionCheck
    {
        /// <summary>
        /// base url for the version check
        /// </summary>
        private const string VersionBaseUrl = "http://www.svenerikmatzen.info";

        /// <summary>
        /// location of the version file relative to the base url
        /// </summary>
        private const string VersionXmlUrl = "/Content/Portals/0/sem.sync.version.xml";

        /// <summary>
        /// name of the calling assembly (the one that did create this object)
        /// </summary>
        private readonly string assemblyName;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionCheck"/> class and initializes the version information.
        /// </summary>
        public VersionCheck()
        {
            var assembly = Assembly.GetCallingAssembly().GetName();
            var myVersion = assembly.Version;
            this.assemblyName = assembly.Name;

            this.Build = myVersion.Build;
            this.Major = myVersion.Major;
            this.MajorRevision = myVersion.MajorRevision;
            this.Minor = myVersion.Minor;
            this.MinorRevision = myVersion.MinorRevision;
            this.Revision = myVersion.Revision;
        }

        /// <summary>
        /// Gets or sets the build number of the version
        /// </summary>
        public int Build { get; set; }

        /// <summary>
        /// Gets or sets the major version number of the version
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        /// Gets or sets the major revision number of the version
        /// </summary>
        public short MajorRevision { get; set; }

        /// <summary>
        /// Gets or sets the minor version number of the version
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        /// Gets or sets the minor revision number of the version
        /// </summary>
        public short MinorRevision { get; set; }

        /// <summary>
        /// Gets or sets the revision number of the version
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// Performs the version check by comparing the own version information with the
        /// version stored at the URL described by <see cref="VersionBaseUrl"/> and <see cref="VersionXmlUrl"/>.
        /// </summary>
        /// <returns>true if the version of this assembly is higher or equal</returns>
        public bool Check()
        {
            return this.Check(null);
        }

        /// <summary>
        /// Performs the version check by comparing the own version information with the
        /// version stored at the URL described by <see cref="VersionBaseUrl"/> and <see cref="VersionXmlUrl"/>.
        /// </summary>
        /// <param name="uiProvider">object that implements the <see cref="IUiInteraction"/> interface to
        /// query information from the user</param>
        /// <returns>true if the version of this assembly is higher or euqal</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "version check is pure optional - in case of a problem we simply skip this")]
        public bool Check(IUiInteraction uiProvider)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[this.assemblyName + "-VersionCheck"];
                bool doCheck;
                if (bool.TryParse(value, out doCheck))
                {
                    if (!doCheck)
                    {
                        return true;
                    }
                }

                var myVersion = new VersionCheck();

                var formatter = new XmlSerializer(typeof(VersionCheck));
                var versionContentFromServer =
                    (new HttpHelper(VersionBaseUrl, false) { UiDispatcher = uiProvider })
                    .GetContent(VersionXmlUrl, "[NOCACHE]");
                var reader = new StringReader(versionContentFromServer);
                var serverVersion = (VersionCheck)formatter.Deserialize(reader);

                return
                    serverVersion.Major <= myVersion.Major &&
                    serverVersion.Minor <= myVersion.Minor &&
                    serverVersion.MajorRevision <= myVersion.MajorRevision &&
                    serverVersion.MinorRevision <= myVersion.MinorRevision &&
                    serverVersion.Build <= myVersion.Build;
            }
            catch 
            {
                // catch simply all - if there's a problem, we will check next time
                return true;
            }
        }

        /// <summary>
        /// Creates a string version number
        /// </summary>
        /// <returns> a string representing the version number </returns>
        public override string ToString()
        {
            return 
                this.Major + "." +
                this.Minor + "." +
                this.MajorRevision + "." +
                this.MinorRevision + "." +
                this.Build;
        }
    }
}