// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rules.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Rules type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sem.GenericHelpers.Contracts.SemRules;

    public static class Rules
    {
        private static Dictionary<Type, object> BouncerRuleCache = new Dictionary<Type, object>();

        public static RuleBase<TData, object> IsNotNull<TData>() where TData : class
        {
            return new RuleBase<TData, object>
            {
                CheckExpression = (data, parameter) => data != null,
            };
        }

        public static RuleBase<TData, Type> ImplementsInterface<TData>() where TData : class
        {
            return new RuleBase<TData, Type>
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

        public static RuleBase<TData, TData[]> IsOneOf<TData>()
        {
            return new RuleBase<TData, TData[]>
                {
                    CheckExpression = (parameterValue, listOfStrings) => listOfStrings.Contains(parameterValue),
                    Message = "The provided value is not one of the expected values",
                };
        }

        public static RuleBase<TData, TData[]> IsNotOneOf<TData>()
        {
            return new RuleBase<TData, TData[]>
                {
                    CheckExpression = (parameterValue, listOfStrings) => !listOfStrings.Contains(parameterValue),
                    Message = "The provided value is not one of the expected values",
                };
        }

        public static RuleBase<int, object> BackEndNumberBoundaries()
        {
            return new RuleBase<int, object>
            {
                CheckExpression = (data, parameter) => data < 16000 && data > -16000,
                Message = "The provided value is not one of the expected values",
            };
        }

        public static RuleBase<object, object> NotNull()
        {
            return new ObjectNotNullRule<object>();
        }

        public static RuleBase<TObject, object> ObjectNotNullRule<TObject>() 
            where TObject : class
        {
            var type = typeof(ObjectNotNullRule<TObject>);
            if (!BouncerRuleCache.ContainsKey(type))
            {
                if (!BouncerRuleCache.ContainsKey(type))
                {
                    BouncerRuleCache.Add(type, new ObjectNotNullRule<TObject>());
                }
            }
            
            return (RuleBase<TObject, object>)BouncerRuleCache[type];
        }
    }
}
