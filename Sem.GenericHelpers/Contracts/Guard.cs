namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Linq;

    public static class Guard 
    {
        public static CheckData<TData> For<TData>(TData data)
        {
            return For(data, "anonymous variable");
        }

        public static CheckData<TData> For<TData>(TData data, string name)
        {
            return new CheckData<TData>
                {
                    ValueName = name,
                    Value = data
                };
        }

        public static Rule<object> IsNotNull = new Rule<object>
        {
            CheckExpression = parameterValue => parameterValue != null,
            ThrowException = (message, parameterName) => new ArgumentNullException(parameterName) 
        };

        public static Rule<string, string[]> IsOneOf = new Rule<string, string[]>
        {
            CheckExpression = (parameterValue, listOfStrings) => listOfStrings.Contains(parameterValue),
            Message = "The provided value is not one of the exprected values",
            ThrowException = (message, parameterName) => new ArgumentOutOfRangeException(parameterName), 
        };

        public static Rule<object, Type> ImplementsInterface = new Rule<object, Type>
            {
                Message = "Parameter does not implement IHandleMessage<> or is null.",
                CheckExpression = (parameterValue, interfaceToImplement) =>
                    parameterValue != null &&
                    parameterValue
                        .GetType()
                        .GetInterfaces()
                        .Where(
                            x => x.GetGenericTypeDefinition() == interfaceToImplement
                                                         || x == interfaceToImplement)
                        .Count() != 0
            };
    }
}
