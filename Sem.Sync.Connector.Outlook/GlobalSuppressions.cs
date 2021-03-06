// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   GlobalSuppressions.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Sem", Scope = "namespace", Target = "Sem.Sync.Connector.Outlook")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Sem")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", 
        MessageId = "System.GC.Collect", Scope = "member", 
        Target = "Sem.Sync.Connector.Outlook.OutlookClient.#CleanupTempFolder()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", 
        MessageId = "System.GC.Collect", Scope = "member", 
        Target = "Sem.Sync.Connector.Outlook.OutlookClient.#GCRelevantCall()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target = "Sem.Sync.Connector.Outlook.OutlookClient.#GetContactsList(Microsoft.Office.Interop.Outlook.Items)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.OutlookClient.#WriteContactToOutlook(Microsoft.Office.Interop.Outlook.Items,Sem.Sync.SyncBase.StdContact,System.Boolean,System.Collections.Generic.List`1<Sem.Sync.Connector.Outlook.ContactsItemContainer>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.CalendarClient.#AddRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.CalendarClient.#MergeMissingRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.CalendarClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.CalendarClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.ContactClient.#AddRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.ContactClient.#DeleteElements(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.ContactClient.#MergeMissingRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.OutlookClient.#ConvertToNativeContact(Sem.Sync.SyncBase.StdContact,Microsoft.Office.Interop.Outlook.ContactItem)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", Target = "Sem.Sync.Connector.Outlook.ContactClient.#RemoveDuplicates(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", Target = "Sem.Sync.Connector.Outlook.CalendarClient.#RemoveDuplicates(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.CalendarClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", 
        Scope = "member", Target = "Sem.Sync.Connector.Outlook.OutlookClient.#GetNamespace()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
        Scope = "namespace", Target = "Sem.Sync.Connector.Outlook")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Outlook.OutlookClient.#ConvertToStandardContact(Microsoft.Office.Interop.Outlook.ContactItem,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdContact>)"
        )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Outlook.CalendarClient.#AddRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Outlook.CalendarClient.#MergeMissingRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Outlook.CalendarClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Outlook.CalendarClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Outlook.ContactClient.#AddRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Outlook.ContactClient.#DeleteElements(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Outlook.ContactClient.#MergeMissingRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Outlook.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Outlook.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.Connector.Outlook.CalendarClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.Connector.Outlook.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Scope = "member", Target = "Sem.Sync.Connector.Outlook.OutlookClient.#ConvertToNativeContact(Sem.Sync.SyncBase.DetailData.StdContact,Microsoft.Office.Interop.Outlook.ContactItem)")]
