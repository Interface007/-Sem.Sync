// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeExternal.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Merge entities (e.g. Contacts, Calendar-Items) using an external tool
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System.Diagnostics;
    using System.IO;

    using Microsoft.Win32;

    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Merge entities (e.g. Contacts, Calendar-Items) using an external tool
    /// </summary>
    public class MergeExternal : SyncComponent, ISyncCommand
    {
        #region Constants and Fields

        /// <summary>
        ///   Determines the registry path to the value that holds the path to BeyondCompare
        /// </summary>
        private const string BeyondComparePath = "Software\\Scooter Software\\Beyond Compare 3";

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the file system path to the external program BeyondCompare
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

        #endregion

        #region Implemented Interfaces

        #region ISyncCommand

        /// <summary>
        /// Merge entities (e.g. Contacts, Calendar-Items) using an external tool
        /// </summary>
        /// <param name="sourceClient">
        /// The source client.
        /// </param>
        /// <param name="targetClient">
        /// The target client.
        /// </param>
        /// <param name="baseliClient">
        /// The baseline client.
        /// </param>
        /// <param name="sourceStorePath">
        /// The source storage path.
        /// </param>
        /// <param name="targetStorePath">
        /// The target storage path.
        /// </param>
        /// <param name="baselineStorePath">
        /// The baseline storage path.
        /// </param>
        /// <param name="commandParameter">
        /// The command parameter.
        /// </param>
        /// <returns>
        /// True if the response from the <see cref="SyncComponent.UiProvider"/> is "continue" 
        /// </returns>
        public bool ExecuteCommand(
            IClientBase sourceClient, 
            IClientBase targetClient, 
            IClientBase baseliClient, 
            string sourceStorePath, 
            string targetStorePath, 
            string baselineStorePath, 
            string commandParameter)
        {
            var pathValue = PathToBeyondCompare;
            if (string.IsNullOrEmpty(pathValue))
            {
                return true;
            }

            var processCommand = " \"" + sourceStorePath + "\"" + " \"" + targetStorePath + "\"" +
                                 (string.IsNullOrEmpty(baselineStorePath)
                                      ? string.Empty
                                      : " \"" + baselineStorePath + "\"");

            var process = Process.Start(pathValue, processCommand);

            if (process != null)
            {
                process.WaitForExit();
            }

            return true;
        }

        #endregion

        #endregion
    }
}