//-----------------------------------------------------------------------
// <copyright file="ProfileIdentifierType.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// Identifies the target system for the profile identifier
    /// </summary>
    public enum ProfileIdentifierType
    {
        /// <summary>
        /// unknown or not registered target system, this defaults to the 
        /// contacts internal id.
        /// </summary>
        Default = 0,

        /// <summary>
        /// target system: www.xing.com
        /// </summary>
        XingProfileId,

        /// <summary>
        /// target system: www.facebook.com
        /// </summary>
        FacebookProfileId,

        /// <summary>
        /// An active directory - this should include an official DNS name.
        /// </summary>
        ActiveDirectoryId,

        /// <summary>
        /// this is the url at the social network side Wer-Kennt-Wen.de.
        /// </summary>
        WerKenntWenUrl,

        /// <summary>
        /// this is a string identifying the contact at the social network side MeinVZ.net.
        /// </summary>
        MeinVZ,

        /// <summary>
        /// target system: www.StayFriends.com
        /// </summary>
        StayFriendsPersonId,

        /// <summary>
        /// Google Mail, Google Contacts, Google Calendar ...
        /// </summary>
        Google,

        /// <summary>
        /// Microsoft Access Database
        /// </summary>
        MicrosoftAccessId,
    }
}