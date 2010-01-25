// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EducationEntry.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the EducationalCertificateType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    /// <summary>
    /// Describes a time span spent to earn some knowledge and/or a certificate
    /// </summary>
    public class EducationEntry
    {
        /// <summary>
        /// Gets or sets the name of the institute that does provide the knowledge.
        /// </summary>
        public string EnducationInstituteName { get; set; }

        /// <summary>
        /// Gets or sets the country where the knowledge has been accquired.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the official name of the certificate.
        /// </summary>
        public string CertificateName { get; set; }

        /// <summary>
        /// Gets or sets the known type of certificate.
        /// </summary>
        public EducationalCertificateType CertificateType { get; set; }

        /// <summary>
        /// Gets or sets the start of the educational process (usually the first day spent at the institute).
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end of the educational process (usually the point in time the certificate has been received).
        /// </summary>
        public DateTime End { get; set; }
    }
}
