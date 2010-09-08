namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class RuleSets
    {
        public static IEnumerable<Rule<TData>> SampleRuleSet<TData>() where TData : class
        {
            var ruleset = new List<Rule<TData>>
                {
                    Rules.IsNotNull<TData>(),

                    new Rule<TData> { CheckExpression = parameterValue => parameterValue.ToString() != "hello", },
                    new Rule<TData> { CheckExpression = parameterValue => !parameterValue.ToString().Contains("'"), },
                    new Rule<TData> { CheckExpression = parameterValue => parameterValue.ToString().Length < 1024, },
                };

            return ruleset;
        }

        public static void RegisterRule<TValue>(Rule<TValue> rule)
        {
            _TypeRegisteredRules.Add(new TypeRule { ValueType = typeof(TValue), Rule = rule });
        }

        private static readonly List<TypeRule> _TypeRegisteredRules = new List<TypeRule>();

        public static IEnumerable<TypeRule> TypeRegisteredRules
        {
            get
            {
                return _TypeRegisteredRules;
            }
        }

        internal static IEnumerable<TypeRule> GetRulesForType(Type type)
        {
            return from x in _TypeRegisteredRules where x.ValueType == type select x;
        }

        public static void RegisterRuleSet<TData>(IEnumerable<Rule<TData>> sampleRuleSet)
        {
            foreach (var rule in sampleRuleSet)
            {
                RegisterRule(rule);
            }
        }
    }
}
