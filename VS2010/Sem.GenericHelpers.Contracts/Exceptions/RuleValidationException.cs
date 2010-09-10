// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleValidationException.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleValidationException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Exceptions
{
    using System;

    public class RuleValidationException : ArgumentException
    {
        public RuleValidationException(Type ruleType, string message, string parameterName)
            : base(message, parameterName)
        {
            this.Rule = ruleType;
        }

        public Type Rule { get; set; }
    }
}