namespace Sem.Sync.Test.Contracts
{
    using System.Collections.Generic;

    using Sem.GenericHelpers.Contracts;

    public class CustomRuleSet : RuleSet<AttributedSampleClass, object>
    {
        protected override List<RuleBase<AttributedSampleClass, object>> GetRuleList()
        {
            return new List<RuleBase<AttributedSampleClass, object>>
                {
                    Rules.IsNotNull<AttributedSampleClass>()
                };
        }
    }
}