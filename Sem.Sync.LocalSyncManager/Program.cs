// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sem.Sync.SharedUI.Common;
using Sem.Sync.SharedUI.WinForms.Tools;

namespace Sem.Sync.LocalSyncManager
{
    using System;
    using System.Windows.Forms;

    using Business;

    using UI;

    /// <summary>
    /// main program execution class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(
                new SyncWizard
                    {
                        DataContext = new SyncWizardContext<UiDispatcher>()
                    });
        }
    }
}
