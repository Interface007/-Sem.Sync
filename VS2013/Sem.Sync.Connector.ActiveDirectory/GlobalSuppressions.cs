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
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Sem")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Sem", Scope = "namespace", Target = "Sem.Sync.ActiveDirectoryConnector")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "Sem.Sync.ActiveDirectoryConnector.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.ActiveDirectoryConnector.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.ActiveDirectoryConnector.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target =
            "Sem.Sync.ActiveDirectoryConnector.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target = "Sem.Sync.ActiveDirectoryConnector.ContactClient.#GetDCs(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target =
            "Sem.Sync.ActiveDirectoryConnector.ContactClient.#DumpUserInformation(System.DirectoryServices.SearchResult,System.String)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target =
            "Sem.Sync.ActiveDirectoryConnector.ContactClient.#ConvertToContact(System.DirectoryServices.SearchResult)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.ActiveDirectory.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.ActiveDirectory.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target =
            "Sem.Sync.Connector.ActiveDirectory.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target = "Sem.Sync.Connector.ActiveDirectory.ContactClient.#GetDCs(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target =
            "Sem.Sync.Connector.ActiveDirectory.ContactClient.#DumpUserInformation(System.DirectoryServices.SearchResult,System.String)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target =
            "Sem.Sync.Connector.ActiveDirectory.ContactClient.#ConvertToContact(System.DirectoryServices.SearchResult)")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.ActiveDirectory.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target =
            "Sem.Sync.Connector.ActiveDirectory.ContactClient.#AddContactsFromAdFilter(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.DirectoryServices.DirectoryEntry)"
        )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.ActiveDirectory.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.Connector.ActiveDirectory.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
