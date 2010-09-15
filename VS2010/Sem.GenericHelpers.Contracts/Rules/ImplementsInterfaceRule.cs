namespace Sem.GenericHelpers.Contracts.Rules
{
    using System;

    public class ImplementsInterfaceRule<TData> : RuleBase<TData, Type>
        where TData : class
    {
        public ImplementsInterfaceRule()
        {
            Message = "Parameter does not implement TInterface or is null.";
            CheckExpression = 
                (data, interfaceToImplement) 
                    => data != null
                    && data.GetType().Implements(interfaceToImplement);
        }
    }
}
