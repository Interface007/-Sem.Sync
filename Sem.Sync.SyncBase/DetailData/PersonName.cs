//-----------------------------------------------------------------------
// <copyright file="PersonName.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    public class PersonName
    {
        public string AcademicTitle { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string LastNamePrefix { get; set; }
        public string Suffix { get; set; }
        public string FormerName { get; set; }
    }
}