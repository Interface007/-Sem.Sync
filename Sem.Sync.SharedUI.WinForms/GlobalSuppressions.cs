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

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#BaseLine")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.MergeEntities.#PerformMerge(System.Collections.Generic.List`1<Sem.Sync.SyncBase.Merging.MergeConflict>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.Tools.UiDispatcher.#PerformAttributeMerge(System.Collections.Generic.List`1<Sem.Sync.SyncBase.Merging.MergeConflict>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#Source")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#Target")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.MatchEntities.#PerformMerge(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.Tools.UiDispatcher.#PerformEntityMerge(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Sem.Sync.SharedUI.WinForms.Tools")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Sem.Sync.SharedUI.WinForms.UI")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Sem.Sync.SharedUI.WinForms.ViewModel")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Scope = "type", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.MatchView")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#CurrentSourceElement")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#CurrentTargetElement")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.Tools.UiDispatcher.#AskForConfirm(System.String,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "baseLine", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#UnMatch(System.Guid)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "BaseLine", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#BaseLine")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.LogOn.#SetLogonCredentials(Sem.GenericHelpers.Interfaces.ICredentialAware,Sem.GenericHelpers.EventArgs.QueryForLogOnCredentialsEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.LogOn.#SetLogonCredentials(Sem.GenericHelpers.Interfaces.ICredentialAware,System.String,System.String,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "unmatch", Scope = "resource", Target = "Sem.Sync.SharedUI.WinForms.UI.MatchEntities.resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sem")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sem", Scope = "namespace", Target = "Sem.Sync.SharedUI.WinForms.Tools")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sem", Scope = "namespace", Target = "Sem.Sync.SharedUI.WinForms.UI")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sem", Scope = "namespace", Target = "Sem.Sync.SharedUI.WinForms.ViewModel")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ui", Scope = "type", Target = "Sem.Sync.SharedUI.WinForms.Tools.UiDispatcher")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#UnMatch(System.Guid)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#Target")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#Source")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#BaseLine")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.Tools.UiDispatcher.#PerformEntityMerge(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.MatchEntities.#PerformMerge(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.MatchEntities.#BtnUnMatch_Click(System.Object,System.EventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.MatchEntities.#BtnMatch_Click(System.Object,System.EventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.MatchEntities.#PerformMatch(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Sem.Sync.SharedUI.WinForms.Controls")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.Controls.ContactAddressView.#Address")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.Controls.ContactCardView.#Contact")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.Tools.UiDispatcher.#ResolveCaptcha(System.String,System.String,Sem.GenericHelpers.Entities.CaptchaResolveRequest)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.MatchEntities.#BtnMatch_Click(System.Object,System.EventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.MatchEntities.#BtnUnMatch_Click(System.Object,System.EventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#SourceAsList()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#TargetAsList()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.ViewModel.Matching.#BaselineAsList()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "svenerikmatzen", Scope = "resource", Target = "Sem.Sync.SharedUI.WinForms.UI.ExceptionOkToSend.resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "automatch", Scope = "resource", Target = "Sem.Sync.SharedUI.WinForms.UI.MatchEntities.resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "Sem.Sync.SharedUI.WinForms.UI.LogOn.#SetLogonCredentials(Sem.GenericHelpers.LogonCredentialRequest)")]
