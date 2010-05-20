// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   main program execution class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.LocalSyncManager
{
    using System;
    using System.Windows.Forms;

    using Sem.GenericHelpers.Exceptions;
    using Sem.Sync.LocalSyncManager.UI;
    using Sem.Sync.SharedUI.Common;
    using Sem.Sync.SharedUI.WinForms.Tools;

    /// <summary>
    /// main program execution class
    /// </summary>
    public static class Program
    {
        #region Public Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ExceptionHandler.UserInterface = new UiDispatcher();
            ExceptionHandler.SendPending();
            ExceptionHandler.ExceptionWriter.ForEach(writer => writer.Clean());

            try
            {
                Application.Run(new SyncWizard { DataContext = new SyncWizardContext(ExceptionHandler.UserInterface) });
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        #endregion
    }
}