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

    using GenericHelpers;

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
        private static readonly Dictionary<string, NodeDescription> Selectors = new Dictionary<string, NodeDescription> 
            {
                { "NameSpace", new NodeDescription(@"//cs:Project/cs:PropertyGroup/cs:RootNamespace", null) },
                { "AssemblyName", new NodeDescription(@"//cs:Project/cs:PropertyGroup/cs:AssemblyName", null) },
                { "Target", new NodeDescription(@"//cs:Project/cs:PropertyGroup/cs:TargetFrameworkVersion", null) },
                {
                    "DebugSymbols",
                    new NodeDescription(
                    @"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:DebugSymbols",
                    (doc, para) =>
                        {
                            var ret = doc.CreateElement("PropertyGroup");
                            ret.Attributes.Append(doc.CreateAttribute("Condition")).Value = string.Format(@" {0} ", para);
                            return ret;
                        },
                    (doc, para) => doc.CreateElement("DebugSymbols")) }, 
                {
                    "OutputPath", 
                    new NodeDescription(@"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:OutputPath", null) },
                { "Constants", new NodeDescription(@"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:DefineConstants", null) },
                { "DebugType", new NodeDescription(@"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:DebugType", null) },
                { "RunCode-Analysis", new NodeDescription(@"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:RunCodeAnalysis", null) },
                { "Optimize", new NodeDescription(@"//cs:Project/cs:PropertyGroup[@Condition="" {0} ""]/cs:Optimize", null) },                  
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

            while (true)
            {
                Console.WriteLine("(C)opy project file settings to CSV file");
                Console.WriteLine("(O)pen file in standard program for *.CSV");
                Console.WriteLine("(W)rite settings from CSV to project files");
                Console.WriteLine("(E)xit program");
                var input = (Console.ReadLine() ?? string.Empty).ToUpperInvariant();
                switch (input)
                {
                    case "C":
                        CopyProjectFilesToCsv(rootFolderPath);
                        break;

                    case "O":
                        System.Diagnostics.Process.Start(Path.Combine(rootFolderPath, "projectsettings.csv"));
                        break;

                    case "W":
                        CopyCsvToProjectFiles(rootFolderPath);
                        break;

                    case "E":
                        return;
                }
            }
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
                        var selector = Selectors.NewIfNull(headers[i]);
                        var parameter = string.Empty;

                        if (headers[i].Contains("..."))
                        {
                            var parts = headers[i].Split(new[] { "..." }, StringSplitOptions.None);
                            selector = Selectors[parts[0]];
                            parameter = ConfigurationConditions[parts[1]];
                        }

                        var value = projectSettings.SelectSingleNode(selector.ProcessedSelector(parameter), namespaceManager);

                        if (value == null)
                        {
                            Console.WriteLine("nonexisting value in file " + Path.GetFileName(columns[0]) + ": " + headers[i]);
                            if (selector.DefaultContent != null)
                            {
                                value = CreateXml(projectSettings, selector, parameter, namespaceManager);
                                value.InnerText = columns[i];
                            }
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

        private static XmlNode CreateXml(XmlDocument document, NodeDescription selector, string defaultNodeParameter, XmlNamespaceManager nameSpaceManager)
        {
            XmlNode node = document.DocumentElement;
            var selectorString = selector.ProcessedSelector(defaultNodeParameter);

            if (selectorString.StartsWith("//"))
            {
                selectorString = selectorString.Substring(2);
                GetFragment(ref selectorString, "/");
            }

            var index = 0;
            do
            {
                if (node == null)
                {
                    return null;
                }

                var localSelector = GetFragment(ref selectorString, "/");
                node = node.SelectSingleNode(localSelector, nameSpaceManager) ??
                       node.AppendChild(selector.DefaultContent[index].Invoke(document, defaultNodeParameter));

                index++;
            }
            while (selectorString.Length > 0);

            return node;
        }

        private static string GetFragment(ref string combinedString, string separator)
        {
            var pos = combinedString.IndexOf(separator);
            var value = combinedString;

            if (pos > 0)
            {
                value = combinedString.Substring(0, pos);
                combinedString = combinedString.Substring(pos + 1);
                return value;
            }

            combinedString = string.Empty;
            return value;
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
                    if (!selector.Value.XPathSelector.Contains("{0}"))
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
                        if (!selector.Value.XPathSelector.Contains("{0}"))
                        {
                            var value = projectSettings.SelectSingleNode(selector.Value.XPathSelector, namespaceManager);
                            outStream.Write(value != null ? value.InnerXml.Replace(';', '+') : string.Empty);
                            outStream.Write(";");
                        }
                        else
                        {
                            foreach (var config in ConfigurationConditions)
                            {
                                var value = projectSettings.SelectSingleNode(string.Format(selector.Value.XPathSelector, config.Value), namespaceManager);
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
