// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Main application class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ContactViewer
{
    using System;
    using System.Windows;

    /// <summary>
    /// Main application class
    /// </summary>
    public partial class App : Application
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "App" /> class.
        /// </summary>
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Error handling method
        /// </summary>
        /// <param name="e">
        /// The exception arguments. 
        /// </param>
        private static void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                var errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval(
                    "throw new Error(\"Unhandled Error in Silverlight 2 Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// handels the application exit event.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The empty event arguments. 
        /// </param>
        private void Application_Exit(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handels the application startup event to initialize vital objects.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The empty event arguments. 
        /// </param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new Page { DataContext = new ViewModel() };
        }

        /// <summary>
        /// Handels application error events
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The error arguments. 
        /// </param>
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                return;
            }

            // NOTE: This will allow the application to continue running after an exception has been thrown
            // but not handled. 
            // For production applications this error handling should be replaced with something that will 
            // report the error to the website and stop the application.
            e.Handled = true;
            Deployment.Current.Dispatcher.BeginInvoke(() => ReportErrorToDOM(e));
        }

        #endregion
    }
}