// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Main execution class that will run the program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.ConsoleClient
{
    using System;
    using System.IO;
    using System.Reflection;

    using Sem.GenericHelpers.EventArgs;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Binding;
    using Sem.Sync.SyncBase.Properties;

    /// <summary>
    /// Main execution class that will run the program.
    /// </summary>
    public static class Program
    {
        #region Public Methods

        /// <summary>
        /// Executes the list of commands specified as a path to the file containing the serialized list in the first parameter.
        /// </summary>
        /// <param name="args">
        /// The parameter that contains the path to the list of serialized commands.
        /// </param>
        public static void Main(string[] args)
        {
#if (DEBUG)
            if (args.Length < 1)
            {
                args = new[] { @"{FS:ApplicationFolder}\A Copy OutlookCal to Xml.SyncList" };
            }

#endif

            if (args.Length >= 1)
            {
                var success = false;
                try
                {
                    var defaultBaseFolder =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SemSyncCmd");
                    Console.WriteLine(Resources.MessageInfoWorkingFolder, defaultBaseFolder);

                    // setup the sync engine
                    var engine = new SyncEngine { WorkingFolder = defaultBaseFolder, UiProvider = new UiDispatcher() };

                    var filename = engine.ReplacePathToken(args[0]);

                    if (!File.Exists(filename))
                    {
                        Console.WriteLine(Resources.ErrorMessageFileNotFound + filename);
                    }
                    else
                    {
                        // load the list of commands
                        Console.WriteLine(Resources.MessageInfoLoadingList, filename);
                        var syncCommands = SyncCollection.LoadSyncList(filename);

                        // feed dispatcher with credentials if specified by the command line
                        ((UiDispatcher)engine.UiProvider).UserDomain = args.Length > 1 ? args[1] : string.Empty;
                        ((UiDispatcher)engine.UiProvider).UserName = args.Length > 2 ? args[2] : string.Empty;
                        ((UiDispatcher)engine.UiProvider).UserPassword = args.Length > 3 ? args[3] : string.Empty;

                        // connect to events
                        engine.ProcessingEvent += ProcessingEvent;
                        engine.ProgressEvent += ProgressEvent;

                        // execute commands
                        success = engine.Execute(syncCommands);

                        // disconnect from events
                        engine.ProcessingEvent -= ProcessingEvent;
                        engine.ProgressEvent -= ProgressEvent;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.MessageErrorException, ex);
                }

                Console.WriteLine(Resources.MessageInfoStatus, success ? "success" : "failed");
            }
            else
            {
                Console.WriteLine(
                    Resources.MessageInfoUsage, 
                    Path.GetFileNameWithoutExtension(Assembly.GetAssembly(typeof(Program)).CodeBase));
            }

#if (DEBUG)

            // wait for user input if in debug build
            Console.WriteLine(Resources.MessageInfoCloseWithEnter);
            Console.ReadLine();
#endif
        }

        #endregion

        #region Methods

        /// <summary>
        /// Logs the processing event
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The processing event arguments that do include the message to be logged. 
        /// </param>
        private static void ProcessingEvent(object sender, ProcessingEventArgs e)
        {
            Console.WriteLine(e.Message + (e.Item == null ? string.Empty : " " + e.Item));
        }

        /// <summary>
        /// Logs the progress event
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The processing event arguments that do include the percentageof work done. 
        /// </param>
        private static void ProgressEvent(object sender, ProgressEventArgs e)
        {
            Console.WriteLine(Resources.MessageInfoProgress, e.PercentageDone);
        }

        #endregion
    }
}