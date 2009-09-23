namespace Sem.GenericTools.ProjectSettings
{
    using System.IO;
    using System.Xml;
    using System.Reflection;
    using System;

    /// <summary>
    /// This program reads and writes project settings from project files
    /// to a flat file in tsv format. This was it's easy to compare the 
    /// settings between different parts of a solution.
    /// </summary>
    class Program
    {
        private static readonly string[] Selectors = new []
                {
                    @"//cs:Project/cs:PropertyGroup/cs:RootNamespace",
                    @"//cs:Project/cs:PropertyGroup/cs:AssemblyName",
                    @"//cs:Project/cs:PropertyGroup/cs:TargetFrameworkVersion",
                    @"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:DebugSymbols",
                    @"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:OutputPath",
                    @"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:DefineConstants",
                    @"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:DebugType",
                };

        private static readonly string[] ConfigurationConditions = new[]
                {
                    @"'$(Configuration)' == ''",
                    @"'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'",
                    @"'$(Configuration)|$(Platform)' == 'Release|AnyCPU'",
                    @"'$(Configuration)|$(Platform)' == 'Debug %28CodeAnalysis%29|AnyCPU'",
                };

        static void Main(string[] args)
        {
            var rootFolderPath = System.AppDomain.CurrentDomain.BaseDirectory;
            if (rootFolderPath.IndexOf(Assembly.GetExecutingAssembly().GetName().Name) > -1)
            {
                rootFolderPath = rootFolderPath.Substring(0, rootFolderPath.IndexOf(Assembly.GetExecutingAssembly().GetName().Name)); // @"C:\CodePlex\SemSync";
            }

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
                        if (!selector.Contains("{0}"))
                        {
                            var value = projectSettings.SelectSingleNode(selector, namespaceManager);
                            outStream.Write(value != null ? value.InnerXml : string.Empty);
                            outStream.Write("\t");
                        }
                        else
                        {
                            foreach (var config in ConfigurationConditions)
                            {
                                var value = projectSettings.SelectSingleNode(string.Format(selector, config), namespaceManager);
                                outStream.Write(value != null ? value.InnerXml : string.Empty);
                                outStream.Write("\t");
                            }
                        }
                    }

                    outStream.WriteLine();
                }

                outStream.Close();
            }

            Console.WriteLine("press ENTER to write the manipulated properties back");
            Console.ReadLine();
        }
    }
}
