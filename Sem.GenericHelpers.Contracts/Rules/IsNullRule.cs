namespace Sem.GenericHelpers.Contracts.Rules
{
    public class IsNullRule<TData>: RuleBase<TData, object>
    where TData: class
    {
        public IsNullRule()
        {
            this.CheckExpression = (target, parameter) => target == null;
            this.Message = "The object is NOT NULL.";
        }
    }
}
