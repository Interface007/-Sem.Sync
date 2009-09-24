// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This program reads and writes project settings from project files
//   to a flat file in tsv format. This was it's easy to compare the
//   settings between different parts of a solution.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericTools.ProjectSettings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    /// <summary>
    /// This program reads and writes project settings from project files
    /// to a flat file in tsv format. This was it's easy to compare the 
    /// settings between different parts of a solution.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// contains the xpath selectors to extract project data
        /// </summary>
        private static readonly Dictionary<string, string> Selectors = new Dictionary<string, string> 
            {
                { "NameSpace", @"//cs:Project/cs:PropertyGroup/cs:RootNamespace" },
                { "AssemblyName", @"//cs:Project/cs:PropertyGroup/cs:AssemblyName" },
                { "Target", @"//cs:Project/cs:PropertyGroup/cs:TargetFrameworkVersion" },
                { "DebugSymbols", @"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:DebugSymbols" },
                { "OutputPath", @"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:OutputPath" },
                { "Constants", @"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:DefineConstants" },
                { "DebugType", @"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:DebugType" },
                { "RunCode-Analysis", @"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:RunCodeAnalysis" }, 
            };

        /// <summary>
        /// contains xpath selectors with parameters to extract configuration specific data
        /// </summary>
        private static readonly Dictionary<string, string> ConfigurationConditions = new Dictionary<string, string>
            {
                { "default", @"'$(Configuration)' == ''" },
                { "debug", @"'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'" },
                { "release", @"'$(Configuration)|$(Platform)' == 'Release|AnyCPU'" },
                { "ca-build", @"'$(Configuration)|$(Platform)' == 'Debug %28CodeAnalysis%29|AnyCPU'" },
                { "ca build", @"'$(Configuration)|$(Platform)' == 'Debug %28Code Analysis%29|AnyCPU'" },
                { "exclude non standard", @"'$(Configuration)|$(Platform)' == 'Exclude Non-Standard-Projects|AnyCPU'" },
            };

        /// <summary>
        /// Performs an export into a tab seperated file and an import back into the project files - the import can be skipped
        /// </summary>
        /// <param name="args"> The command line arguments. </param>
        public static void Main(string[] args)
        {
            var rootFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            if (rootFolderPath.IndexOf(Assembly.GetExecutingAssembly().GetName().Name) > -1)
            {
                rootFolderPath = rootFolderPath.Substring(0, rootFolderPath.IndexOf(Assembly.GetExecutingAssembly().GetName().Name));
            }

            CopyProjectFilesToCsv(rootFolderPath);

            var ask = true;
            while (ask)
            {
                Console.WriteLine("(W)rite or (E)xit or (O)open file?");
                var input = (Console.ReadLine() ?? string.Empty).ToUpperInvariant();
                switch (input)
                {
                    case "O":
                        System.Diagnostics.Process.Start(Path.Combine(rootFolderPath, "projectsettings.csv"));
                        break;

                    case "W":
                        ask = false;
                        break;

                    case "E":
                        return;
                }
            }

            CopyCsvToProjectFiles(rootFolderPath);
        }

        /// <summary>
        /// updates all project files in a folder including the sub folders with the settings of a csv file
        /// </summary>
        /// <param name="rootFolderPath"> The root folder path. </param>
        private static void CopyCsvToProjectFiles(string rootFolderPath)
        {
            using (var inStream = new StreamReader(Path.Combine(rootFolderPath, "projectsettings.csv")))
            {
                var line = inStream.ReadLine();
                var headers = line.Split(';');

                while (line.Length > 0)
                {
                    line = inStream.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    var columns = line.Split(';');

                    XmlNamespaceManager namespaceManager;
                    var projectSettings = GetProjectSettings(columns[0], out namespaceManager);
                    
                    for (var i = 1; i < headers.Length; i++)
                    {
                        string selector;
                        if (headers[i].Contains("..."))
                        {
                            var parts = headers[i].Split(new[] { "..." }, StringSplitOptions.None);
                            selector = string.Format(Selectors[parts[0]], ConfigurationConditions[parts[1]]);
                        }
                        else
                        {
                            selector = Selectors[headers[i]];
                        }

                        var value = projectSettings.SelectSingleNode(selector, namespaceManager);
                        
                        if (value == null)
                        {
                            Console.WriteLine("nonexisting value in file " + Path.GetFileName(columns[0]) + ": " + headers[i]);

                            // TODO: we need to generate the xml nodes if they are missing.
                            ////value.InnerText = columns[i];
                        }
                        else
                        {
                            value.InnerText = columns[i].Replace("+", ";");
                        }
                    }

                    projectSettings.Save(columns[0]);
                }
            }
        }

        /// <summary>
        /// reads the projects files in a folder including all sub folders and exports selected properties into a csv file
        /// </summary>
        /// <param name="rootFolderPath"> The root folder path. </param>
        private static void CopyProjectFilesToCsv(string rootFolderPath)
        {
            using (var outStream = new StreamWriter(Path.Combine(rootFolderPath, "projectsettings.csv")))
            {
                outStream.Write("filename;");
                foreach (var selector in Selectors)
                {
                    if (!selector.Value.Contains("{0}"))
                    {
                        outStream.Write(selector.Key.Replace(';', '+'));
                        outStream.Write(";");
                    }
                    else
                    {
                        foreach (var config in ConfigurationConditions)
                        {
                            outStream.Write((selector.Key + "..." + config.Key).Replace(';', '+'));
                            outStream.Write(";");
                        }
                    }
                }

                outStream.WriteLine();

                foreach (var projectFile in Directory.GetFiles(rootFolderPath, "*.csproj", SearchOption.AllDirectories))
                {
                    XmlNamespaceManager namespaceManager;
                    var projectSettings = GetProjectSettings(projectFile, out namespaceManager);

                    outStream.Write(projectFile + ";");
                    foreach (var selector in Selectors)
                    {
                        if (!selector.Value.Contains("{0}"))
                        {
                            var value = projectSettings.SelectSingleNode(selector.Value, namespaceManager);
                            outStream.Write(value != null ? value.InnerXml.Replace(';', '+') : string.Empty);
                            outStream.Write(";");
                        }
                        else
                        {
                            foreach (var config in ConfigurationConditions)
                            {
                                var value = projectSettings.SelectSingleNode(string.Format(selector.Value, config.Value), namespaceManager);
                                outStream.Write(value != null ? value.InnerXml.Replace(';', '+') : string.Empty);
                                outStream.Write(";");
                            }
                        }
                    }

                    outStream.WriteLine();
                }

                outStream.Close();
            }
        }

        /// <summary>
        /// reads the project file into a document
        /// </summary>
        /// <param name="projectFile"> The project file. </param>
        /// <param name="namespaceManager"> The namespace manager. </param>
        /// <returns> an xml document with the content of the project file </returns>
        private static XmlDocument GetProjectSettings(string projectFile, out XmlNamespaceManager namespaceManager)
        {
            var projectSettings = new XmlDocument();
            projectSettings.Load(projectFile);
            namespaceManager = new XmlNamespaceManager(projectSettings.NameTable);
            namespaceManager.AddNamespace("cs", "http://schemas.microsoft.com/developer/msbuild/2003");
            return projectSettings;
        }
    }
}
