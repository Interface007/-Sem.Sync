// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   GlobalSuppressions.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
        Scope = "namespace", Target = "Sem.Sync.SyncBase.Attributes")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
        Scope = "namespace", Target = "Sem.Sync.SyncBase.Binding")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
        Scope = "namespace", Target = "Sem.Sync.SyncBase.EventArgs")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
        Scope = "namespace", Target = "Sem.Sync.SyncBase.Interfaces")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
        Scope = "namespace", Target = "Sem.Sync.SyncBase.Merging")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.SyncTools.#BuildConflictTestContainerList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Type)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.SyncTools.#DetectConflicts(System.Collections.Generic.List`1<Sem.Sync.SyncBase.Merging.ConflictTestContainer>,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", 
        "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", 
        Target = "Sem.Sync.SyncBase.Helpers.SyncTools.#LoadFromFile`1(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", 
        "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", 
        Target = "Sem.Sync.SyncBase.Helpers.Factory.#GetNewObject`1(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.Extensions.#LoadFrom`1(System.Collections.Generic.List`1<!!0>,System.String,System.Type[])"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.Extensions.#SaveTo`1(System.Collections.Generic.List`1<!!0>,System.String,System.Type[])"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", 
        Scope = "member", Target = "Sem.Sync.SyncBase.SyncEngine.#Execute(Sem.Sync.SyncBase.SyncDescription)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
        Scope = "member", Target = "Sem.Sync.SyncBase.SyncEngine.#Execute(Sem.Sync.SyncBase.SyncDescription)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
        Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#MergeHighEvidence`1(!!0,!!0,System.Type)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.SyncTools.#DetectConflicts(Sem.Sync.SyncBase.Merging.ConflictTestContainer,System.Boolean)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.VCardConverter.#VCardToStdContact(System.Byte[],Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.SyncComponent.#LogProcessingEvent(System.Object,Sem.Sync.SyncBase.EventArgs.ProcessingEventArgs)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", 
        MessageId = "street", Scope = "member", 
        Target = "Sem.Sync.SyncBase.Helpers.SyncTools.#ExtractStreetNumberExtension(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.SyncTools.#LoadFromFile`1(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.ReplacementLists.#BusinessCompanyName")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.ReplacementLists.#BusinessHomepage")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.VersionCheck.#Check()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", 
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", 
        Target = "Sem.Sync.SyncBase.SyncEngine.#MergeFiles(System.String,System.String,System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
        Scope = "namespace", Target = "Sem.Sync.SyncBase.Helpers")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.SyncComponent.#LogProcessingEvent(System.Object,Sem.GenericHelpers.EventArgs.ProcessingEventArgs)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", 
        Scope = "member", Target = "Sem.Sync.SyncBase.ImageEntry.#ImageData")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#BusinessCertificates")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#BusinessHistoryEntries")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#Contacts")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#ImageEntries")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#LanguageKnowledges")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.DetailData.ProfileIdInformation.#op_Explicit(Sem.Sync.SyncBase.DetailData.ProfileIdInformation):System.String"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.DetailData.ProfileIdInformation.#op_Implicit(System.String):Sem.Sync.SyncBase.DetailData.ProfileIdInformation"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"
        , Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.Extensions.#MergeList(System.Collections.Generic.List`1<System.Collections.Generic.KeyValuePair`2<System.String,Sem.Sync.SyncBase.DetailData.ProfileIdInformation>>,System.Collections.Generic.List`1<System.Collections.Generic.KeyValuePair`2<System.String,Sem.Sync.SyncBase.DetailData.ProfileIdInformation>>)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", 
        Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.ImageEntry.#ImageData")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdCalendarItem.#OptionalAttendees")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdCalendarItem.#RequiredAttendees")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#SourceSpecificAttributes")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdCalendarItem.#Resources")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
        Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.DetailData.ProfileIdentifiers.#MatchesAny(Sem.Sync.SyncBase.DetailData.ProfileIdentifiers)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", 
        "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", 
        Target = "Sem.Sync.SyncBase.StdClient.#GetColumnDefinition`1(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#BusinessCertificates")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#BusinessHistoryEntries")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#Contacts")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#ImageEntries")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#GetColumnDefinition(System.String,System.Type)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.ReplacementLists.#BusinessCompanyName")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.ReplacementLists.#BusinessHomepage")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", 
        Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.ReplacementLists.#City")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", 
        MessageId = "0#", Scope = "member", 
        Target = "Sem.Sync.SyncBase.WebScrapingBaseClient.#ConvertToStdContact(System.String,System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", 
        MessageId = "0#", Scope = "member", Target = "Sem.Sync.SyncBase.WebScrapingBaseClient.#.ctor(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", 
        Scope = "member", Target = "Sem.Sync.SyncBase.WebSideParameters.#ExtractorProfilePictureUrl")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", 
        Scope = "member", Target = "Sem.Sync.SyncBase.WebSideParameters.#HttpUrlBaseAddress")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", 
        Scope = "member", Target = "Sem.Sync.SyncBase.WebSideParameters.#HttpUrlContactDownload")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", 
        Scope = "member", Target = "Sem.Sync.SyncBase.WebSideParameters.#HttpUrlLogOnRequest")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", 
        Scope = "member", Target = "Sem.Sync.SyncBase.WebSideParameters.#ImagePlaceholderUrl")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", 
        Scope = "member", Target = "Sem.Sync.SyncBase.WebSideParameters.#HttpUrlFriendList")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "vCard", Scope = "member", 
        Target = "Sem.Sync.SyncBase.Helpers.VCardConverter.#VCardToStdContact(System.Byte[])")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "vCard", Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.VCardConverter.#VCardToStdContact(System.Byte[],Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "AmericanSamoa", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#AmericanSamoa")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "BosniaHerzegovina", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#BosniaHerzegovina")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "BruneiDarussalam", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#BruneiDarussalam")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "BurkinaFaso", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#BurkinaFaso")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "CaymanIslands", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#CaymanIslands")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "ChristmasIsland", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#ChristmasIsland")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "CookIslands", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#CookIslands")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "CostaRica", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#CostaRica")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "CzechRepublic", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#CzechRepublic")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "DiegoGarcia", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#DiegoGarcia")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "DominicanRepublic", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#DominicanRepublic")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "DominicanRepublic", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#DominicanRepublic2")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "EasterIsland", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#EasterIsland")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "EastTimor", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#EastTimor")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "ElSalvador", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#ElSalvador")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "EMail", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.ProfileIdentifierType.#GenericEMail")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "EquatorialGuinea", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#EquatorialGuinea")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "FaroeIslands", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#FaroeIslands")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "FijiIslands", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#FijiIslands")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "FrenchAntilles", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#FrenchAntilles")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "FrenchGuiana", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#FrenchGuiana")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "FrenchPolynesia", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#FrenchPolynesia")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "GaboneseRepublic", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#GaboneseRepublic")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "GuantanamoBay", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#CubaGuantanamoBay")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "GuantanamoBay", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#GuantanamoBay")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "GuineaBissau", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#GuineaBissau")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "HongKong", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#HongKong")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "KoreaNorth", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#KoreaNorth")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "KoreaSouth", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#KoreaSouth")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "KyrgyzRepublic", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#KyrgyzRepublic")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "MaliRepublic", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#MaliRepublic")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "MarshallIslands", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#MarshallIslands")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "MayotteIsland", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#MayotteIsland")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "MidwayIsland", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#MidwayIsland")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "NetherlandsAntilles", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#NetherlandsAntilles")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "NewCaledonia", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#NewCaledonia")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "NewZealand", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#ChathamIslandNewZealand")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "NewZealand", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#NewZealand")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "NorfolkIsland", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#NorfolkIsland")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "PalestinianSettlements", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#PalestinianSettlements")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "PuertoRico", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#PuertoRico")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "PuertoRico", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#PuertoRico2")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "RéunionIsland", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#RéunionIsland")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "RussiaKazakhstan", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#RussiaKazakhstan")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "RwandeseRepublic", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#RwandeseRepublic")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "SanMarino", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#SanMarino")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "SaudiArabia", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#SaudiArabia")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "SeychellesRepublic", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#SeychellesRepublic")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "SierraLeone", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#SierraLeone")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "SlovakRepublic", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#SlovakRepublic")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "SolomonIslands", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#SolomonIslands")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "SouthAfrica", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#SouthAfrica")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "SriLanka", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#SriLanka")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "StHelena", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#StHelena")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "StLucia", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#StLucia")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "TimorLeste", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#TimorLeste")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "TogoleseRepublic", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#TogoleseRepublic")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "TongaIslands", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#TongaIslands")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "TrinidadTobago", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#TrinidadTobago")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "UnitedKingdom", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#UnitedKingdom")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "VaticanCity", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#VaticanCity")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "VCard", Scope = "member", 
        Target = "Sem.Sync.SyncBase.Helpers.VCardConverter.#StdContactToVCard(Sem.Sync.SyncBase.StdContact)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "VCard", Scope = "member", 
        Target = "Sem.Sync.SyncBase.Helpers.VCardConverter.#VCardToStdContact(System.Byte[])")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "VCard", Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.VCardConverter.#VCardToStdContact(System.Byte[],Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "VCard", Scope = "type", Target = "Sem.Sync.SyncBase.Helpers.VCardConverter")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "WakeIsland", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#WakeIsland")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "Logon", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.ConnectorInformation.#LogonCredentials")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "Logon", Scope = "member", 
        Target = "Sem.Sync.SyncBase.Interfaces.IClientBase.#QueryForLogonCredentialsEvent")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", 
        MessageId = "WorkFlow", Scope = "type", Target = "Sem.Sync.SyncBase.DetailData.SyncWorkFlow")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly"
        , MessageId = "unmatching", Scope = "resource", Target = "Sem.Sync.SyncBase.Properties.Resources.resources")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Cocos", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#CocosKeelingIslands")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Coted", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#CotedIvoireIvoryCoast")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Denormalized", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.PhoneNumber.#DenormalizedPhoneNumber")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Diplom", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.EducationalCertificateType.#Diplom")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Ellipso", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#EllipsoMobileSatelliteservice")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Enducation", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.EducationEntry.#EnducationInstituteName")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Fach", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.EducationalCertificateType.#FachHochschulReife")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Freephone", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#InternationalFreephoneService")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Globalstar", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#GlobalstarMobileSatelliteService")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Hauptschul", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.EducationalCertificateType.#HauptschulAbschluss")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Hochschul", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.EducationalCertificateType.#FachHochschulReife")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Hochschul", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.EducationalCertificateType.#HochschulReife")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Inmarsat", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#InmarsatAtlanticOceanEast")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Inmarsat", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#InmarsatAtlanticOceanWest")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Inmarsat", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#InmarsatIndianOcean")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Inmarsat", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#InmarsatPacificOcean")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Inmarsat", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#InmarsatSNAC")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Ivoire", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#CotedIvoireIvoryCoast")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Leste", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#TimorLeste")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Realschul", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.EducationalCertificateType.#RealschulAbschluss")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Reife", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.EducationalCertificateType.#FachHochschulReife")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Reife", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.EducationalCertificateType.#HochschulReife")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Repof", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#CongoDemRepof")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Repof", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#MacedoniaFormerYugoslavRepof")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Satelliteservice", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#EllipsoMobileSatelliteservice")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Satelliteservice", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#EMSATMobileSatelliteservice")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Satelliteservice", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#IridiumMobileSatelliteservice")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Satelliteservice", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#ThurayaMobileSatelliteservice")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Statesof", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#UnitedStatesofAmericaCanada")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Thuraya", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#ThurayaMobileSatelliteservice")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Tomeand", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#SaoTomeandPrincipe"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Turksand", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#TurksandCaicosIslands")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Vordiplom", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.EducationalCertificateType.#Vordiplom")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Wallisand", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#WallisandFutunaIslands")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "v", Scope = "member", 
        Target = "Sem.Sync.SyncBase.Helpers.VCardConverter.#VCardToStdContact(System.Byte[])")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "Indetifier", Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.VCardConverter.#VCardToStdContact(System.Byte[],Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
        MessageId = "v", Scope = "member", 
        Target =
            "Sem.Sync.SyncBase.Helpers.VCardConverter.#VCardToStdContact(System.Byte[],Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)"
        )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "El", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#ElSalvador")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "EMSAT", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#EMSATMobileSatelliteservice")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "GMSS", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#GlobalMobileSatelliteSystemGMSS")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "ICO", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#ICOGlobalMobileSatelliteService")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "ISCS", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#InternationalSharedCostServiceISCS")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "PRC", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#ChinaPRC")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "SNAC", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#InmarsatSNAC")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "St", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#StHelena")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "St", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#StKittsNevis")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "St", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#StLucia")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "St", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#StPierreMiquelon")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "St", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#StVincentGrenadines")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "unknown", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#unknown175")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "unspecified", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#unspecified")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "UPT", Scope = "member", 
        Target = "Sem.Sync.SyncBase.DetailData.CountryCode.#UniversalPersonalTelecommunicationsUPT")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "Ui", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.ISyncCommand.#UiProvider")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "Ui", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#UiDispatcher")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "Ui", Scope = "member", Target = "Sem.Sync.SyncBase.SyncComponent.#UiProvider")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
        MessageId = "Ui", Scope = "type", Target = "Sem.Sync.SyncBase.Interfaces.IUiSyncInteraction")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix"
        , Scope = "type", Target = "Sem.Sync.SyncBase.SourceSpecificAttribute")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#LanguageKnowledge")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.ReplacementLists.#City")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdCalendarItem.#OptionalAttendees")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdCalendarItem.#RequiredAttendees")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IUiSyncInteraction.#PerformAttributeMerge(System.Collections.Generic.List`1<Sem.Sync.SyncBase.Merging.MergeConflict>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#ToStdCalendarItems(System.Collections.Generic.IEnumerable`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#ToStdContacts(System.Collections.Generic.IEnumerable`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#MergeHighEvidence(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#ToMatchingEntries(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#ToStdElements`1(System.Collections.Generic.IEnumerable`1<!!0>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IClientBase.#AddRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IClientBase.#DeleteElements(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IClientBase.#GetAll(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IClientBase.#MergeMissingRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IClientBase.#WriteRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IUiSyncInteraction.#PerformEntityMerge(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#AddRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#CleanUpEntities(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#DeleteElements(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#GetAll(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#MergeMissingRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#Normalize(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#WriteRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.WebScrapingBaseClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#MergeList(System.Collections.Generic.List`1<System.String>,System.Collections.Generic.List`1<System.String>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdCalendarItem.#Resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdContact.#Categories")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#LoadFrom`1(System.Collections.Generic.List`1<!!0>,System.String,System.Type[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#SaveTo`1(System.Collections.Generic.List`1<!!0>,System.String,System.Type[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#SaveToString`1(System.Collections.Generic.List`1<!!0>,System.Type[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#ToOtherType`2(System.Collections.Generic.List`1<!!0>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.StdContact.#BusinessCertificates")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.StdContact.#BusinessHistoryEntries")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.SyncTools.#BuildConflictTestContainerList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.Type)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.StdContact.#Contacts")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.StdContact.#ImageEntries")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#MergeList(System.Collections.Generic.List`1<System.Collections.Generic.KeyValuePair`2<System.String,Sem.Sync.SyncBase.DetailData.ProfileIdInformation>>,System.Collections.Generic.List`1<System.Collections.Generic.KeyValuePair`2<System.String,Sem.Sync.SyncBase.DetailData.ProfileIdInformation>>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.StdContact.#LanguageKnowledge")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#ToMatchingEntries(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.StdCalendarItem.#OptionalAttendees")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.StdCalendarItem.#RequiredAttendees")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IUiSyncInteraction.#PerformAttributeMerge(System.Collections.Generic.List`1<Sem.Sync.SyncBase.Merging.MergeConflict>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#ToStdCalendarItems(System.Collections.Generic.IEnumerable`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#ToStdContacts(System.Collections.Generic.IEnumerable`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.Extensions.#MergeHighEvidence(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IClientBase.#AddRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IClientBase.#DeleteElements(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IClientBase.#MergeMissingRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IClientBase.#WriteRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.Interfaces.IUiSyncInteraction.#PerformEntityMerge(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,Sem.Sync.SyncBase.DetailData.ProfileIdentifierType)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#AddRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#CleanUpEntities(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#DeleteElements(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#MergeMissingRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#Normalize(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#WriteFullList(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.StdClient.#WriteRange(System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.WebScrapingBaseClient.#ReadFullList(System.String,System.Collections.Generic.List`1<Sem.Sync.SyncBase.DetailData.StdElement>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.StdCalendarItem.#Resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.StdContact.#Categories")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "Sem.Sync.SyncBase.DetailData.StdContact.#NormalizeContent()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "VCard", Scope = "member", Target = "Sem.Sync.SyncBase.Helpers.VCardConverter.#StdContactToVCard(Sem.Sync.SyncBase.DetailData.StdContact)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "Sem.Sync.SyncBase.WebScrapingBaseClient.#Logon()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Winforms", Scope = "member", Target = "Sem.Sync.SyncBase.Attributes.ClientStoragePathDescriptionAttribute.#WinformsConfigurationClass")]
