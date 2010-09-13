// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleValidationResult.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleValidationResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;

    public class RuleValidationResult
    {
        public RuleValidationResult(Type ruleType, string message, string valueName, bool result)
        {
            this.Result = result;
            this.RuleType = ruleType;
            this.Message = message;
            this.ValueName = valueName;
        }

        public bool Result { get; protected set; }

        public string Message { get; protected set; }

        public Type RuleType { get; protected set; }

        public string ValueName { get; protected set; }

        public override string ToString()
        {
            return this.Message;
        }
    }
}