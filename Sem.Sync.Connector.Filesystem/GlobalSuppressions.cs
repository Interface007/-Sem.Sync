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
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.ContactClientIndividualFiles.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.ContactClientIndividualFiles.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.ContactClientVCards.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.ContactClientVCards.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.GenericClient`1.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.GenericClient`1.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.GenericClientCsv`1.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.GenericClientCsv`1.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Filesystem.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Filesystem")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Filesystem", Scope = "namespace", Target = "Sem.Sync.Connector.Filesystem")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Sem")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Sem", Scope = "namespace", Target = "Sem.Sync.Connector.Filesystem")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", 
        MessageId = "string", Scope = "member", 
        Target = "Sem.Sync.Connector.Filesystem.ColumnDefinition.#.ctor(System.String,System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", 
        MessageId = "System.Boolean.TryParse(System.String,System.Boolean@)", Scope = "member", 
        Target = "Sem.Sync.Connector.Filesystem.ContactClientVCards.#.ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "VCards", Scope = "type", Target = "Sem.Sync.Connector.Filesystem.ContactClientVCards")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.ContactClientIndividualFiles.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.ContactClientIndividualFiles.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.ContactClientVCards.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.ContactClientVCards.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.GenericClient`1.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.GenericClient`1.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.GenericClientCsv`1.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.GenericClientCsv`1.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.Connector.Filesystem.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
