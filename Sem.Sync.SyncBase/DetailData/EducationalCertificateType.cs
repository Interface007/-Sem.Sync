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
        /// <summary>
        /// This person is know to not own any official educational certificate
        /// </summary>
        NoOfficialCertificate,

        /// <summary>
        /// There is no knowledge about the person whether she/he owns an official educational certificate
        /// </summary>
        UnknownCertificate,

        /// <summary>
        /// There is knowledge about this person to hold the explicit official "Hauptschul-Abschluss" certificate.
        /// This does not include the implicit holding of this certificate by holding a more valuable certificate.
        /// </summary>
        HauptschulAbschluss,

        /// <summary>
        /// There is knowledge about this person to hold the explicit official "Realschul-Abschluss (Mittlere Reife)" certificate.
        /// This does not include the implicit holding of this certificate by holding a more valuable certificate.
        /// </summary>
        RealschulAbschluss,

        /// <summary>
        /// There is knowledge about this person to hold the explicit official "Fach-Hochschul-Reife" certificate.
        /// This does not include the implicit holding of this certificate by holding a more valuable certificate.
        /// </summary>
        FachHochschulReife,
        
        /// <summary>
        /// There is knowledge about this person to hold the explicit official "Allgemeine Hauptschul-Abschluss" certificate.
        /// This does not include the implicit holding of this certificate by holding a more valuable certificate.
        /// </summary>
        HochschulReife,

        /// <summary>
        /// There is knowledge about this person to hold the explicit official "Vordiplom" certificate.
        /// This does not include the implicit holding of this certificate by holding a more valuable certificate.
        /// </summary>
        Vordiplom,

        /// <summary>
        /// There is knowledge about this person to hold the explicit official "Bachelor" certificate.
        /// This does not include the implicit holding of this certificate by holding a more valuable certificate.
        /// </summary>
        Bachelor,

        /// <summary>
        /// There is knowledge about this person to hold the explicit official "Diplom" certificate.
        /// This does not include the implicit holding of this certificate by holding a more valuable certificate.
        /// </summary>
        Diplom,

        /// <summary>
        /// There is knowledge about this person to hold the explicit official "Master" certificate.
        /// This does not include the implicit holding of this certificate by holding a more valuable certificate.
        /// </summary>
        Master,
    }
}