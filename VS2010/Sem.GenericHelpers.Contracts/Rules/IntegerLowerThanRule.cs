// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringRegexMatchRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the StringRegexMatchRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    public class IntegerLowerThanRule : RuleBase<int, int>
    {
        public IntegerLowerThanRule()
        {
            this.CheckExpression = (target, parameter) => target < parameter;
            this.Message = "The argument must be lower than >>{0}<<.";
        }
    }
}
