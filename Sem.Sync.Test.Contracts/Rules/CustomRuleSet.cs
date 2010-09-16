namespace Sem.Sync.Test.Contracts.Rules
{
    using System.Collections.Generic;

    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Sync.Test.Contracts.Entities;

    public class CustomRuleSet : RuleSet<AttributedSampleClass, object>
    {
        protected override IEnumerable<RuleBase<AttributedSampleClass, object>> GetRuleList()
        {
            return new List<RuleBase<AttributedSampleClass, object>>
                {
                    new IsNotNullRule<AttributedSampleClass>()
                };
        }
    }
}