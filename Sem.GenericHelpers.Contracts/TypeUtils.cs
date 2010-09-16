namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rules;

    internal static class TypeUtils
    {
        internal static bool Implements(this Type toTest, Type interfaceToImplement)
        {
            return (from i in toTest.GetInterfaces()
                    where (i.IsGenericType && i.GetGenericTypeDefinition() == interfaceToImplement)
                          || i == interfaceToImplement
                    select i).Count() != 0;
        }
    }
}
