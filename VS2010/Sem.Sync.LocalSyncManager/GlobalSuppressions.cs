// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   GlobalSuppressions.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.LocalSyncManager.ClientViewModel.#SyncCommandLists")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", 
        MessageId = "System.Reflection.Assembly.LoadFile", Scope = "member", 
        Target = "Sem.Sync.LocalSyncManager.Business.SyncWizardContext.#.ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.LocalSyncManager.Business.SyncWizardContext.#SyncWorkflowsTemplates")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.LocalSyncManager.Business.SyncWizardContext.#SyncWorkflowData")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.LocalSyncManager.Business.ClientViewModel.#SyncCommandLists")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.LocalSyncManager.Business.ClientViewModel.#SyncCommandLists")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "WorkFlow", Scope = "type", Target = "Sem.Sync.LocalSyncManager.Entities.SyncWorkFlow")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1301:AvoidDuplicateAccelerators", 
        Scope = "type", Target = "Sem.Sync.LocalSyncManager.UI.SyncWizard")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"
        , Scope = "member", Target = "Sem.Sync.LocalSyncManager.Business.SyncWizardContext.#ClientsTarget")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"
        , Scope = "member", Target = "Sem.Sync.LocalSyncManager.Business.SyncWizardContext.#ClientsSource")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
        Scope = "member", 
        Target = "Sem.Sync.LocalSyncManager.UI.SyncWizard.#SyncWizard_Load(System.Object,System.EventArgs)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", 
        Scope = "member", Target = "Sem.Sync.LocalSyncManager.UI.SyncWizard.#RunCommands()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Windows.Forms.FileDialog.set_Filter(System.String)", Scope = "member", Target = "Sem.Sync.LocalSyncManager.UI.SyncWizard.#AskForDestinationFile(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Sem.Sync.LocalSyncManager.UI.Commands.#LocalSync_Load(System.Object,System.EventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Sem.Sync.LocalSyncManager.Program.#Main()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Sem.Sync.LocalSyncManager.UI.SyncWizard.#RunCommands()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Sem.Sync.LocalSyncManager.UI.SyncWizard.#SyncWizard_Load(System.Object,System.EventArgs)")]
