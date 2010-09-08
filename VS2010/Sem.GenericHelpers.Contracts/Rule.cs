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
        public void AssertFor(CheckData<TValue> value, TCheckParameter checkParameter)
        {
            this.InvokeInternal(() => this.CheckExpression(value.Value, checkParameter), value.ValueName);
        }

        [Conditional("DEBUG")]
        public void AssumeFor(CheckData<TValue> value, TCheckParameter checkParameter)
        {
            this.InvokeInternal(() => this.CheckExpression(value.Value, checkParameter), value.ValueName);
        }
    }

    public class Rule<TValue> : RuleBase<Func<TValue, bool>>
    {
        public void AssertFor(CheckData<TValue> value)
        {
            this.InvokeInternal(() => this.CheckExpression(value.Value), value.ValueName);
        }

        [Conditional("DEBUG")]
        public void AssumeFor(CheckData<TValue> value)
        {
            this.InvokeInternal(() => this.CheckExpression(value.Value), value.ValueName);
        }
    }
}