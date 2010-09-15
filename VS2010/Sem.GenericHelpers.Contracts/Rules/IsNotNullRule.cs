namespace Sem.GenericHelpers.Contracts.Rules
{
    public class IsNotNullRule<TData>: RuleBase<TData, object>
    where TData: class
    {
        public IsNotNullRule()
        {
            this.CheckExpression = (target, parameter) => target != null;
            this.Message = "The object is NULL.";
        }
    }
}
