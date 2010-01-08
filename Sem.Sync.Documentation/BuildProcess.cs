// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildProcess.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Hooks into the DocProject build process for the project in which it's defined.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Documentation
{
    using System;
    using System.IO;
    using System.Text;

    using DaveSexton.DocProject;
    using DaveSexton.DocProject.Engine;

    /// <summary>
    /// Hooks into the DocProject build process for the project in which it's defined.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class must be registered with the DocProject in the <em>Active Projects</em>
    /// tools options page in order for DocProject to instantiate it during a help build.
    /// </para>
    /// <para>
    /// To cancel the build at any time call the <see cref="BuildContext.Cancel" /> 
    /// method.  The build process will end after the current step is executed, 
    /// unless the step is being executed in the background.  In that case, it may 
    /// end immediately.
    /// </para>
    /// <para>
    /// Note: Do not cache instances of the <see cref="BuildContext" /> class.  A new 
    /// <see cref="BuildContext" /> is created each time the project is built.
    /// </para>
    /// </remarks>
    public class BuildProcess : BuildProcessComponent
    {
        /// <summary>
        /// Start time of the build process
        /// </summary>
        private DateTime buildStart;

        /// <summary>
        /// Start time of the build step
        /// </summary>
        private DateTime stepStart;

        /// <summary>
        /// Called before the project's help build starts.
        /// </summary>
        /// <param name="context">Provides information about the build process.</param>
        public override void BuildStarting(BuildContext context)
        {
            // Uncomment the following line to break into the debugger: 
            ////System.Diagnostics.Debugger.Break();

            var dir = context.ProjectDirectory;
            var folder = Path.Combine(dir, "buildhelp");

            foreach (var item in Directory.GetFiles(folder, "*.*"))
            {
                File.SetAttributes(item, File.GetAttributes(item) & !FileAttributes.ReadOnly);
            }

            this.buildStart = DateTime.Now;
        }

        /// <summary>
        /// Called before a <paramref name="step" /> is executed during a help build.
        /// </summary>
        /// <param name="step"><see cref="IBuildStep" /> implementation to be executed.</param>
        /// <param name="context">Provides information about the build process.</param>
        /// <returns><b>true</b> indicates that the process should continue, otherwise, 
        /// <b>false</b> indicates that the process should skip this step.</returns>
        public override bool BeforeExecuteStep(IBuildStep step, BuildContext context)
        {
            var dir = context.ProjectDirectory;
            var file = Path.Combine(dir, "Help\\Sem_Sync_Documentation.hhp");

            string fileContent = File.ReadAllText(file);
            if (String.IsNullOrEmpty(fileContent) == false)
            {
                File.WriteAllText(file, fileContent, new UTF8Encoding(false));
            }

            this.stepStart = DateTime.Now;
            return true;
        }

        /// <summary>
        /// Called after a <paramref name="step" /> has been executed during a help build.
        /// </summary>
        /// <param name="step"><see cref="IBuildStep" /> implementation that was executed.</param>
        /// <param name="context">Provides information about the build process.</param>
        public override void AfterExecuteStep(IBuildStep step, BuildContext context)
        {
            this.TraceLine();
            this.TraceLine("Step {0} Time Elapsed: {1}", context.CurrentStepIndex + 1, DateTime.Now - this.stepStart);
        }

        /// <summary>
        /// Called after the project's help build has finished.
        /// </summary>
        /// <remarks>
        /// The <see cref="BuildContext.Cancel" /> method has no affect at this 
        /// point in the build process.  This method is the final step before the 
        /// build statistics are displayed.
        /// <para>
        /// This method is always invoked if <see cref="BuildStarting" /> is invoked, 
        /// regardless of whether an exception is thrown in any of the other methods, 
        /// <see cref="BuildContext.Cancel" /> has been called, or an exeception has
        /// been thrown by the build engine.
        /// </para>
        /// <para>
        /// To determine whether a help build failed or succeeded, examine the value of the
        /// <see cref="BuildContext.BuildState" /> property.
        /// </para>
        /// </remarks>
        /// <param name="context">Provides information about the build process.</param>
        public override void BuildCompleted(BuildContext context)
        {
            this.TraceLine();
            this.TraceLine("Total Time Elapsed: {0}", DateTime.Now - this.buildStart);
        }
    }
}
