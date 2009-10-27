// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EducationalCertificateType.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the EducationalCertificateType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// Describes the level of educational certification. This includes any qualifying certificate from school
    /// that is gathered in a sequence of the "school career". Similar certificates must be insterted in a 
    /// sequence. The higher the value of the enumeration value, the higher the "value of the certificate".
    /// </summary>
    public enum EducationalCertificateType
    {
        NoOfficialCertificate,
        UnknownCertificate,
        HauptschulAbschluss,
        RealschulAbschluss,
        FachHochschulReife,
        HochschulReife,
        Vordiplom,
        Bachelor,
        Diplom,
        Master,
    }
}