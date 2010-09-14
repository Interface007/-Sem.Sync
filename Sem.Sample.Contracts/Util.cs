namespace Sem.Sample.Contracts
{
    using System;

    using Sem.GenericHelpers.Contracts.RuleExecuters;

    internal static class Util
    {
        internal static void PrintEntries(MessageCollection<MyCustomer> results)
        {
            foreach (var result in results.Results)
            {
                Console.WriteLine("----------");
                Console.WriteLine(result);
            }

            Console.WriteLine("----------");
        }

    }
}
