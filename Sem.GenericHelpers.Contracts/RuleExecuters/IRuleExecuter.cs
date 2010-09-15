namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using Sem.GenericHelpers.Contracts.Attributes;

    public interface IRuleExecuter
    {
        /// <summary>
        /// Evaluates the rule check expression for a rule attribute (property or method attribute).
        /// The attributes of methods and properties need to be executed by reflection, so we need a 
        /// public method for that purpose.
        /// </summary>
        /// <param name="ruleExecuter">The rule executer (inherits from <see cref="RuleExecuter{TData,TResultClass}"/>) that will invoke the rule validation.</param>
        /// <param name="ruleAttribute">The attribute that defines the rule.</param>
        /// <param name="propertyName">The name of the data to be validated by the rule.</param>
        /// <returns>A new instance of <see cref="RuleValidationResult"/>.</returns>
        RuleValidationResult InvokeRuleExecutionForAttribute(IRuleExecuter ruleExecuter, ContractRuleAttribute ruleAttribute, string propertyName);
    }
}