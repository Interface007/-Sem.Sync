// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMessageCollection.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IMessageCollection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System.Collections.Generic;

    public interface IMessageCollection
    {
        List<RuleValidationResult> Results { get; }
    }
}