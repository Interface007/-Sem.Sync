// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sem")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sem", Scope = "namespace", Target = "Sem.Sync.ActiveDirectoryConnector")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.ActiveDirectoryConnector.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.ActiveDirectoryConnector.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.ActiveDirectoryConnector.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Sem.Sync.ActiveDirectoryConnector.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Sem.Sync.ActiveDirectoryConnector.ContactClient.#GetDCs(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Sem.Sync.ActiveDirectoryConnector.ContactClient.#DumpUserInformation(System.DirectoryServices.SearchResult,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Sem.Sync.ActiveDirectoryConnector.ContactClient.#ConvertToContact(System.DirectoryServices.SearchResult)")]
