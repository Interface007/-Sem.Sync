namespace Sem.GenericHelpers.Contracts.Rules
{
    using System.Linq;

    public class IsNotOneOfRule<TData> : RuleBase<TData, TData[]>
        where TData : class
    {
        public IsNotOneOfRule()
        {
            CheckExpression = (data, listOfStrings) => !listOfStrings.Contains(data);
            Message = "The provided value is not one of the expected values";
        }
    }
}
