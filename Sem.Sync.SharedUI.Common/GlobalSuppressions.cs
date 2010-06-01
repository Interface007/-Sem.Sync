// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   GlobalSuppressions.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", 
        Scope = "member", Target = "Sem.Sync.SharedUI.Common.Config.#LastUsedSyncTemplateData")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "Ui", Scope = "type", Target = "Sem.Sync.SharedUI.Common.SyncWizardContext`1")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"
        , Scope = "member", Target = "Sem.Sync.SharedUI.Common.SyncWizardContext`1.#ClientsSource")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"
        , Scope = "member", Target = "Sem.Sync.SharedUI.Common.SyncWizardContext`1.#ClientsTarget")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes"
        , Scope = "member", Target = "Sem.Sync.SharedUI.Common.SyncWizardContext`1.#OpenExceptionFolder()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target = "Sem.Sync.SharedUI.Common.SyncWizardContext`1.#OpenExceptionFolder()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
        Scope = "namespace", Target = "Sem.Sync.SharedUI.Common")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "Ui", Scope = "member", Target = "Sem.Sync.SharedUI.Common.SyncWizardContext.#UiProvider")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SharedUI.Common.SyncWizardContext.#SyncWorkflowsTemplates")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SharedUI.Common.SyncWizardContext.#SyncWorkflowData")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Sem.Sync.SharedUI.Common.SyncWizardContext.#ClientsSource")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Sem.Sync.SharedUI.Common.SyncWizardContext.#ClientsTarget")]
