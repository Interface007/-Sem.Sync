// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanResultContainer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the BooleanResultContainer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Cloud
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Implements a boolean result class that inherits some standard properties from <see cref="ResultBase"/>
    /// </summary>
    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class BooleanResultContainer : ResultBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the result of the method call was true or false.
        /// </summary>
        [DataMember]
        public bool Result { get; set; }
    }
}
