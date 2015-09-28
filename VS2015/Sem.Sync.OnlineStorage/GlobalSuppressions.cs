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
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.OnlineStorage.ContactListContainer.#ContactList")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.OnlineStorage.ContactListContainer.#ContactList")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", 
        Scope = "member", Target = "Sem.Sync.OnlineStorage.ViewContact.#Picture")]