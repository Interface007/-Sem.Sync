// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringMaxLengthRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the StringMaxLengthRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    public class StringMaxLengthRule : RuleBase<string, int>
    {
        public StringMaxLengthRule()
        {
            this.CheckExpression = (target, parameter) => target != null && target.Length <= parameter;
            this.Message = "The string does not have the maximum length of >>{0}<<.";
        }
    }
}