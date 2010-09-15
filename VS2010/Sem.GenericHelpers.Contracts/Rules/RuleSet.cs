// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleSet.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleSet type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using System.Collections;
    using System.Collections.Generic;

    public abstract class RuleSet<TData, TParameter> : IEnumerable<RuleBase<TData, TParameter>>
    {
        public IEnumerator<RuleBase<TData, TParameter>> GetEnumerator()
        {
            return this.GetRuleList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
     
        protected abstract List<RuleBase<TData, TParameter>> GetRuleList();
    }
}
