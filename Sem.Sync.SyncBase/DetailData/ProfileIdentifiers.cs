//-----------------------------------------------------------------------
// <copyright file="ProfileIdentifiers.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    public enum ProfileIdentifierType
    {
        Default = 0,
        XingProfileId,
        FacebookProfileId,
        ActiveDirectoryId,
    }
        
    public class ProfileIdentifiers
    {
        public string XingProfileId { get; set; }
        public string FacebookProfileId { get; set; }
        public string ActiveDirectoryId { get; set; }
    }
}