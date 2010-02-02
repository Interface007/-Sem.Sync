// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncCollection.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Binding
{
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;
    
    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// implements a binding list for the SyncDescription class
    /// </summary>
    public class SyncCollection : BindingList<SyncDescription>
    {
        /// <summary>
        /// loads a SyncCollection from the file system.
        /// </summary>
        /// <param name="pathToFile">path to the file to load</param>
        /// <returns>a SyncCollection loaded from the disk</returns>
        public static SyncCollection LoadSyncList(string pathToFile)
        {
            if (Path.GetExtension(pathToFile).ToUpperInvariant() == ".DSYNCLIST")
            {
                var workFlow = Tools.LoadFromFile<SyncWorkFlow>(pathToFile);

                var commandsFileName = workFlow
                    .ReplaceToken(workFlow.Template)
                    .Replace("{FS:ApplicationFolder}", Path.GetDirectoryName(Assembly.GetCallingAssembly().CodeBase));

                if (commandsFileName.StartsWith("file:\\", System.StringComparison.OrdinalIgnoreCase))
                {
                    commandsFileName = commandsFileName.Substring(6);
                }

                var commands = LoadSyncList(commandsFileName);

                if (commands == null)
                {
                    return null;
                }

                foreach (var command in commands)
                {
                    command.SourceCredentials = (command.SourceConnector != null && command.SourceConnector == "{source}") ? workFlow.Source.LogonCredentials : command.SourceCredentials;
                    command.SourceCredentials = (command.SourceConnector != null && command.SourceConnector == "{target}") ? workFlow.Target.LogonCredentials : command.SourceCredentials;
                    command.TargetCredentials = (command.TargetConnector != null && command.TargetConnector == "{source}") ? workFlow.Source.LogonCredentials : command.TargetCredentials;
                    command.TargetCredentials = (command.TargetConnector != null && command.TargetConnector == "{target}") ? workFlow.Target.LogonCredentials : command.TargetCredentials;

                    command.SourceConnector = workFlow.ReplaceToken(command.SourceConnector);
                    command.TargetConnector = workFlow.ReplaceToken(command.TargetConnector);
                    command.SourceStorePath = workFlow.ReplaceToken(command.SourceStorePath);
                    command.TargetStorePath = workFlow.ReplaceToken(command.TargetStorePath);
                    command.CommandParameter = workFlow.ReplaceToken(command.CommandParameter);
                }

                return commands;
            }

            using (var file = new FileStream(pathToFile, FileMode.Open, FileAccess.Read))
            {
                var formatter = new XmlSerializer(typeof(SyncCollection));
                var result = (SyncCollection)formatter.Deserialize(file);

                return result;
            }
        }

        /// <summary>
        /// overrides the AddNewCore method to create the correct object instance
        /// </summary>
        /// <returns>a new item of the SyncDescription class</returns>
        protected override object AddNewCore()
        {
            var newItem = new SyncDescription();
            this.Add(newItem);
            return newItem;
        }
    }
}