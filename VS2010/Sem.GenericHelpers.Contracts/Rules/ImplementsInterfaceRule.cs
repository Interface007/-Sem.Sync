namespace Sem.GenericHelpers.Contracts.Rules
{
    using System;
    using System.Linq;

    public class ImplementsInterfaceRule<TData> : RuleBase<TData, Type>
        where TData : class
    {
        public ImplementsInterfaceRule()
        {
            Message = "Parameter does not implement TInterface or is null.";
            CheckExpression = 
                (data, interfaceToImplement) 
                    => data != null 
                    && (from i in data.GetType().GetInterfaces()
                        where (i.IsGenericType && i.GetGenericTypeDefinition() == interfaceToImplement) 
                              || i == interfaceToImplement
                        select i).Count() != 0;
        }
    }
}
