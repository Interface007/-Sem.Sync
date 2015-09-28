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
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#GetStandardContactList(System.Boolean)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#GetVariableContactList()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Test.DataGenerator.Contacts.#SerializeList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdContact>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Test.DataGenerator.Contacts.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Test.DataGenerator.Contacts.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#VariableContactList")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#GetMatchingBaseline()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#GetMatchingSource()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#GetMatchingTarget()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", 
        Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#GetMatchingBaseline()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", 
        Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#GetMatchingTarget()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", 
        Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#GetMatchingSource()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#SerializeList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdContact>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Test.DataGenerator.Contacts.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
