// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleSets.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleSets type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sem.GenericHelpers.Contracts.Attributes;

    public static class RuleSets
    {
        internal static readonly IList<KeyValuePair<Type, RuleBaseInformation>> TypeRegisteredRules = new List<KeyValuePair<Type, RuleBaseInformation>>();

        public static void RegisterRule<TData, TParameter>(RuleBase<TData, TParameter> rule)
        {
            TypeRegisteredRules.Add(new KeyValuePair<Type, RuleBaseInformation>(typeof(TData), rule));
        }

        public static void RegisterRuleSet<TData, TParameter>(IEnumerable<RuleBase<TData, TParameter>> sampleRuleSet)
        {
            foreach (var rule in sampleRuleSet)
            {
                RegisterRule(rule);
            }
        }

        public static IEnumerable<RuleBase<TData, TParameter>> GetRulesForType<TData, TParameter>()
        {
            var valueType = typeof(TData);

            // build a list of "registered" rules
            var rulesForType = (from x in TypeRegisteredRules
                                where x.Key == valueType
                                select x.Value as RuleBase<TData, TParameter>).ToList();

            // get all class-level rule-attributes and enumerate to build list of rules
            // to be excuted for this object instance.
            var attribs = valueType.GetCustomAttributes(typeof(ContractRuleAttribute), true);
            foreach (ContractRuleAttribute attrib in attribs)
            {
                var ruleSet = attrib.Type.GetConstructor(new Type[] { }).Invoke(null) as RuleSet<TData, TParameter>;
                if (ruleSet == null)
                {
                    continue;
                }

                rulesForType.AddRange(ruleSet);
            }

            return rulesForType;
        }
    }
}
