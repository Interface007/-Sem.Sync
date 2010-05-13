// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusinessCertificate.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Describes a single earned business certificate like "Microsoft Certified Technology Specialist WinForms Developer"
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    /// <summary>
    /// Describes a single earned business certificate like "Microsoft Certified Technology Specialist WinForms Developer"
    /// </summary>
    [Serializable]
    public class BusinessCertificate
    {
        /// <summary>
        /// Gets or sets official complete name of the certificate.
        /// </summary>
        public string CertificateName { get; set; }

        /// <summary>
        /// Gets or sets the name when this certificate has been earned - usually printed on the certificate.
        /// </summary>
        public DateTime CertificationDate { get; set; }
    }
}
