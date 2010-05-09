﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactReference.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class describes a relationship between two contacts. It's intended to
//   connect two "known" contacts to each other. It's NOT intended to describe
//   "real world relationship status" like marriage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    /// <summary>
    /// This class describes a relationship between two contacts. It's intended to 
    /// connect two "known" contacts to each other. It's NOT intended to describe 
    /// "real world relationship status" like marriage.
    /// </summary>
    public class ContactReference
    {
        /// <summary>
        /// Gets or sets the id of the target contact.
        /// </summary>
        public Guid Target { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a business relation between the two contacts.
        /// </summary>
        public bool IsBusinessContact { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a private relation between the two contacts.
        /// </summary>
        public bool IsPrivateContact { get; set; }
    }
}