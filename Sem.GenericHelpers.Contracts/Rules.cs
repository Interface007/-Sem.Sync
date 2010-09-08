namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Linq;

    public static class Rules
    {
        public static Rule<TData> IsNotNull<TData>() where TData : class
        {
            return new Rule<TData>
            {
                CheckExpression = parameterValue => parameterValue != null,
                ThrowException = (message, parameterName) => new ArgumentNullException(parameterName)
            };
        }

        public static Rule<object> IsNotNullObject() 
        {
            return new Rule<object>
            {
                CheckExpression = parameterValue => parameterValue != null,
                ThrowException = (message, parameterName) => new ArgumentNullException(parameterName)
            };
        }

        public static Rule<TData, Type> ImplementsInterface<TData>() where TData : class
        {
            return new Rule<TData, Type>
            {
                Message = "Parameter does not implement TInterface or is null.",
                CheckExpression = (parameterValue, interfaceToImplement) =>
                    parameterValue != null &&
                    parameterValue
                        .GetType()
                        .GetInterfaces()
                        .Where(
                            x => (x.IsGenericType && x.GetGenericTypeDefinition() == interfaceToImplement)
                                || x == interfaceToImplement
                            )
                        .Count() != 0
            };
        }

        public static Rule<TData, TData[]> IsOneOf<TData>()
        {
            return new Rule<TData, TData[]>
                {
                    CheckExpression = (parameterValue, listOfStrings) => listOfStrings.Contains(parameterValue),
                    Message = "The provided value is not one of the expected values",
                    ThrowException = (message, parameterName) => new ArgumentOutOfRangeException(parameterName),
                };
        }

        public static Rule<TData, TData[]> IsNotOneOf<TData>()
        {
            return new Rule<TData, TData[]>
                {
                    CheckExpression = (parameterValue, listOfStrings) => !listOfStrings.Contains(parameterValue),
                    Message = "The provided value is not one of the expected values",
                    ThrowException = (message, parameterName) => new ArgumentOutOfRangeException(parameterName),
                };
        }

        public static Rule<int> BackEndNumberBoundaries()
        {
            return new Rule<int>
            {
                CheckExpression = parameterValue => parameterValue < 16000 && parameterValue > -16000,
                Message = "The provided value is not one of the expected values",
                ThrowException = (message, parameterName) => new ArgumentOutOfRangeException(parameterName, "the backend currently only supports values between -16000 and 16000"),
            };
        }
    }
}
