// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Config.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Configuration class to read configuration data from the app.config and the registry
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.LocalSyncManager.Tools
{
    using System;
    using System.IO;

    /// <summary>
    /// Configuration class to read configuration data from the app.config and the registry
    /// </summary>
    internal static class Config
    {
        #region Constants and Fields

        /// <summary>
        ///   registry path to store credentials
        /// </summary>
        private const string RegBasePath = "Software\\Sem.Sync\\LocalSyncManager";

        /// <summary>
        ///   Determine the default data folder (base of all file sytem paths)
        /// </summary>
        private static readonly string DefaultBaseFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SemSyncManager");

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets last used SyncTemplateData.
        /// </summary>
        public static string LastUsedSyncTemplateData
        {
            get
            {
                return GenericHelpers.Tools.GetRegValue(RegBasePath, "LastUsedSyncTemplateData", null);
            }

            set
            {
                GenericHelpers.Tools.SetRegValue(RegBasePath, "LastUsedSyncTemplateData", value);
            }
        }

        /// <summary>
        ///   Gets or sets the folder to store data in file system
        /// </summary>
        public static string WorkingFolder
        {
            get
            {
                // read the folder from app.config
                var folder = (string)Properties.Settings.Default["WorkingFolder"];

                // default to base folder + "\Work" and save to app.config
                if (string.IsNullOrEmpty(folder))
                {
                    folder = Path.Combine(DefaultBaseFolder, "Work");
                    WorkingFolder = folder;
                }

                return folder;
            }

            set
            {
                // set the folder and save to app.config
                Properties.Settings.Default["WorkingFolder"] = value;
                Properties.Settings.Default.Save();
            }
        }

        #endregion
    }
}