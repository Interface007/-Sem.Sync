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
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Google.ContactClient.#DeleteElements(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Google.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Google.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Google.GoogleContactMappingExtensions.#UpdatePhoto(Google.Contacts.Contact,Sem.Sync.SyncBase.StdContact,Sem.GenericHelpers.Interfaces.ICredentialAware)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Google.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Google.GoogleContactMappingExtensions.#SetAddress(Sem.Sync.SyncBase.StdContact,Google.GData.Extensions.PostalAddress)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Google.GoogleContactMappingExtensions.#SetPhone(Sem.Sync.SyncBase.StdContact,Google.GData.Extensions.PhoneNumber)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "Sem.Sync.Connector.Google.GoogleContactMappingExtensions.#UpdatePhoto(Google.Contacts.Contact,Sem.Sync.SyncBase.StdContact,Sem.Sync.Connector.Google.ContactClient)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", 
        MessageId = "addressType", Scope = "member", 
        Target =
            "Sem.Sync.Connector.Google.GoogleContactMappingExtensions.#AddIMAddress(Google.Contacts.Contact,Sem.Sync.SyncBase.DetailData.InstantMessengerAddresses,System.String)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", 
        MessageId = "stdBusinessDepartment", Scope = "member", 
        Target =
            "Sem.Sync.Connector.Google.GoogleContactMappingExtensions.#AddOrganization(Google.Contacts.Contact,System.String,System.String,System.String)"
        )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Google.ContactClient.#DeleteElements(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Google.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.Connector.Google.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.Connector.Google.ContactClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Sem.Sync.Connector.Google.GoogleContactMappingExtensions.#UpdatePhoto(Google.Contacts.Contact,Sem.Sync.SyncBase.DetailData.StdContact,Sem.Sync.Connector.Google.ContactClient)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "member", Target = "Sem.Sync.Connector.Google.ContactClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
