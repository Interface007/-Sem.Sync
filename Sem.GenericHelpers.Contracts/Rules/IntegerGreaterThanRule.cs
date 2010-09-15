namespace Sem.GenericHelpers.Contracts.Rules
{
    public class IntegerGreaterThanRule : RuleBase<int, int>
    {
        public IntegerGreaterThanRule()
        {
            this.CheckExpression = (target, parameter) => target > parameter;
            this.Message = "The argument must be greater than >>{0}<<.";
        }
    }
}