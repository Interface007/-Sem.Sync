//-----------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Sem.Sync.SyncBase.Attributes")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Sem.Sync.SyncBase.Binding")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Sem.Sync.SyncBase.EventArgs")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Sem.Sync.SyncBase.Interfaces")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Sem.Sync.SyncBase.Merging")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.SyncTools.#BuildConflictTestContainerList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Type)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.SyncTools.#DetectConflicts(System.Collections.Generic.List`1<Sem.Sync.SyncBase.Merging.ConflictTestContainer>,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.SyncTools.#LoadFromFile`1(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Factory.#GetNewObject`1(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#LoadFrom`1(System.Collections.Generic.List`1<!!0>,System.String,System.Type[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#SaveTo`1(System.Collections.Generic.List`1<!!0>,System.String,System.Type[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Scope = "member", Target = "Sem.Sync.SyncBase.SyncEngine.#Execute(Sem.Sync.SyncBase.SyncDescription)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "Sem.Sync.SyncBase.SyncEngine.#Execute(Sem.Sync.SyncBase.SyncDescription)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#MergeHighEvidence`1(!!0,!!0,System.Type)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.SyncTools.#DetectConflicts(Sem.Sync.SyncBase.Merging.ConflictTestContainer,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.VCardConverter.#VCardToStdContact(System.Byte[],Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "member", Target = "Sem.Sync.SyncBase.SyncComponent.#LogProcessingEvent(System.Object,Sem.Sync.SyncBase.EventArgs.ProcessingEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "street", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.SyncTools.#ExtractStreetNumberExtension(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.SyncTools.#LoadFromFile`1(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.ReplacementLists.#BusinessCompanyName")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.ReplacementLists.#BusinessHomepage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.VersionCheck.#Check()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Sem.Sync.SyncBase.SyncEngine.#MergeFiles(System.String,System.String,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Sem.Sync.SyncBase.Helpers")]
