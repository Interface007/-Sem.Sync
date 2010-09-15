// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringMinLengthRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the StringMinLengthRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    public class StringMinLengthRule : RuleBase<string, int>
    {
        public StringMinLengthRule()
        {
            this.CheckExpression = (target, parameter) => target != null && target.Length >= parameter;
            this.Message = "The string does not have the minimum length of >>{0}<<.";
        }
    }
}