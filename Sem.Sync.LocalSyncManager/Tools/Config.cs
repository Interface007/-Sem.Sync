namespace Sem.Sync.LocalSyncManager.Tools
{
    using System;
    using System.IO;

    /// <summary>
    /// Configuration class to read configuration data from the app.config and the registry
    /// </summary>
    static class Config
    {
        /// <summary>
        /// Determine the default data folder (base of all file sytem paths)
        /// </summary>
        private static readonly string defaultBaseFolder = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SemSyncManager");

        /// <summary>
        /// Gets or sets the folder to store data in file system
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
                    folder = Path.Combine(defaultBaseFolder, "Work");
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
    }
}