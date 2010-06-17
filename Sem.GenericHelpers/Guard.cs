using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sem.GenericHelpers
{
    public static class Guard
    {
        public static void NotNull<T>(T parameterToCheck, string parameterName)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
