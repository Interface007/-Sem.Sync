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
        public RuleValidationResult(Type ruleType, string message, string valueName)
        {
            this.RuleType = ruleType;
            this.Message = message;
            this.ValueName = valueName;
        }

        protected string Message { get; set; }

        protected Type RuleType { get; set; }

        protected string ValueName { get; set; }

        public override string ToString()
        {
            return this.Message;
        }
    }
}