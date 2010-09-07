// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Rule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Diagnostics;

    public class Rule<TValue, TCheckParameter> : RuleBase<Func<TValue, TCheckParameter, bool>>
    {
        public void AssertFor(string parameterName, TValue parameterValue, TCheckParameter checkParameter)
        {
            this.InvokeInternal(() => this.CheckExpression(parameterValue, checkParameter), parameterName);
        }

        [Conditional("DEBUG")]
        public void AssumeFor(string parameterName, TValue parameterValue, TCheckParameter checkParameter)
        {
            this.InvokeInternal(() => this.CheckExpression(parameterValue, checkParameter), parameterName);
        }
    }

    public class Rule<TValue> : RuleBase<Func<TValue, bool>>
    {
        public void AssertFor(string parameterName, TValue parameterValue)
        {
            this.InvokeInternal(() => this.CheckExpression(parameterValue), parameterName);
        }

        [Conditional("DEBUG")]
        public void AssumeFor(string parameterName, TValue parameterValue)
        {
            this.InvokeInternal(() => this.CheckExpression(parameterValue), parameterName);
        }
    }
}