namespace Sem.GenericHelpers.Contracts.Rules
{
    using System.Linq;

    public class IsOneOfRule<TData> : RuleBase<TData, TData[]>
        where TData : class
    {
        public IsOneOfRule()
        {
            CheckExpression = (parameterValue, listOfStrings) => listOfStrings.Contains(parameterValue);
            Message = "The provided value is not one of the expected values";
        }
    }
}
