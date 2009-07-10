// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeExternal.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MergeExternal type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System.Diagnostics;
    using System.IO;

    using Interfaces;
    using Microsoft.Win32;
    
    /// <summary>
    /// Merge entities (e.g. Contacts, Calendar-Items) using an external tool
    /// </summary>
    public class MergeExternal : ISyncCommand
    {
        /// <summary>
        /// Determines the registry path to the value that holds the path to BeyondCompare
        /// </summary>
        private const string BeyondComparePath = "Software\\Scooter Software\\Beyond Compare 3";

        /// <summary>
        /// Gets or sets UiProvider.
        /// </summary>
        public IUiInteraction UiProvider { get; set; }

        /// <summary>
        /// Gets the file system path to the external program BeyondCompare
        /// </summary>
        private static string PathToBeyondCompare
        {
            get
            {
                var key = Registry.LocalMachine.OpenSubKey(BeyondComparePath);
                if (key != null)
                {
                    var pathValue = key.GetValue("ExePath");
                    if (pathValue != null)
                    {
                        if (File.Exists(pathValue.ToString()))
                        {
                            return pathValue.ToString();
                        }
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Merge entities (e.g. Contacts, Calendar-Items) using an external tool
        /// </summary>
        /// <param name="sourceClient">The source client.</param>
        /// <param name="targetClient">The target client.</param>
        /// <param name="baseliClient">The baseline client.</param>
        /// <param name="sourceStorePath">The source storage path.</param>
        /// <param name="targetStorePath">The target storage path.</param>
        /// <param name="baselineStorePath">The baseline storage path.</param>
        /// <param name="commandParameter">The command parameter.</param>
        /// <returns> True if the response from the <see cref="UiProvider"/> is "continue" </returns>
        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            var pathValue = PathToBeyondCompare;
            if (string.IsNullOrEmpty(pathValue))
            {
                return true;
            }

            var processCommand = " \"" + sourceStorePath + "\"" + " \"" + targetStorePath + "\"" +
                                 (string.IsNullOrEmpty(baselineStorePath) ? string.Empty : " \"" + baselineStorePath + "\"");

            var process = Process.Start(
                pathValue,
                processCommand);

            if (process != null)
            {
                process.WaitForExit();
            }

            return true;
        }
    }
}
