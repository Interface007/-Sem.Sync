﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneNumberType.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Type of phone number like "the phone at home" etc.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System.Xml.Serialization;

    /// <summary>
    /// Type of phone number like "the phone at home" etc.
    /// </summary>
    public enum PhoneNumberType
    {
        /// <summary>
        /// Home phone number - to reach the person at home
        /// </summary>
        [XmlEnum(Name = "home")]
        Home,

        /// <summary>
        /// Mobile phone number - to reach the person via mobile phone
        /// </summary>
        [XmlEnum(Name = "mobile")]
        Mobile,

        /// <summary>
        /// Work phone number - to reach the person at work
        /// </summary>
        [XmlEnum(Name = "work")]
        Work
    }
}