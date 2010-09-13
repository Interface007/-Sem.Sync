// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleSets.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleSets type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class RuleSets
    {
        public static IEnumerable<RuleBase<TData, object>> SampleRuleSet<TData>() where TData : class
        {
            var ruleset = new List<RuleBase<TData, object>>
                {
                    Rules.IsNotNull<TData>(),

                    new RuleBase<TData, object> { CheckExpression = (data, parameter) => data.ToString() != "hello", },
                    new RuleBase<TData, object> { CheckExpression = (data, parameter) => !data.ToString().Contains("'"), },
                    new RuleBase<TData, object> { CheckExpression = (data, parameter) => data.ToString().Length < 1024, },
                };

            return ruleset;
        }

        public static void RegisterRule<TData, TParameter>(RuleBase<TData, TParameter> rule)
        {
            _TypeRegisteredRules.Add(new TypeRule { ValueType = typeof(TData), Rule = rule });
        }

        private static readonly List<TypeRule> _TypeRegisteredRules = new List<TypeRule>();

        public static IEnumerable<TypeRule> TypeRegisteredRules
        {
            get
            {
                return _TypeRegisteredRules;
            }
        }

        internal static IEnumerable<TypeRule> GetRulesForType<TData, TParameter>()
        {
            var valueType = typeof(TData);

            // build a list of "registered" rules
            var rulesForType = (from x in _TypeRegisteredRules
                                where x.ValueType == valueType
                                select x).ToList();

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

                rulesForType.AddRange(ruleSet.Select(rule => new TypeRule { Rule = rule, ValueType = valueType }));
            }

            return rulesForType;
        }

        public static void RegisterRuleSet<TData, TParameter>(IEnumerable<RuleBase<TData, TParameter>> sampleRuleSet)
        {
            foreach (var rule in sampleRuleSet)
            {
                RegisterRule(rule);
            }
        }
    }
}
