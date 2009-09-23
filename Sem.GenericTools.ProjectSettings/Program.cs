namespace Sem.GenericTools.ProjectSettings
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// This program reads and writes project settings from project files
    /// to a flat file in tsv format. This was it's easy to compare the 
    /// settings between different parts of a solution.
    /// </summary>
    class Program
    {
        private static readonly string[] Selectors = new []
                {
                    @"//cs:Project/cs:PropertyGroup/cs:Configuration",
                    @"//cs:Project/cs:PropertyGroup/cs:RootNamespace",
                    @"//cs:Project/cs:PropertyGroup/cs:AssemblyName",
                    @"//cs:Project/cs:PropertyGroup/cs:TargetFrameworkVersion",
                    @"//cs:Project/cs:PropertyGroup[@cs:Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ""]/cs:DebugSymbols",
                };

        static void Main(string[] args)
        {
            var rootFolderPath = @"C:\CodePlex\SemSync";

            using (var outStream = new StreamWriter(Path.Combine(rootFolderPath, "projectsettings.csv")))
            {
                foreach (var projectFile in Directory.GetFiles(rootFolderPath, "*.csproj", SearchOption.AllDirectories))
                {
                    var projectSettings = new XmlDocument();
                    projectSettings.Load(projectFile);
                    var namespaceManager = new XmlNamespaceManager(projectSettings.NameTable);
                    namespaceManager.AddNamespace("cs", "http://schemas.microsoft.com/developer/msbuild/2003");


                    foreach (var selector in Selectors)
                    {
                        var value = projectSettings.SelectSingleNode(selector, namespaceManager);
                        outStream.Write(value != null ? value.InnerXml : string.Empty);
                        outStream.Write("\t");
                    }

                    outStream.WriteLine();
                }

                outStream.Close();
            }
        }
    }
}
