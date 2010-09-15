namespace Sem.Sample.Contracts
{
    using System;

    using Sem.GenericHelpers.Contracts.RuleExecuters;
    using Sem.Sample.Contracts.Entities;

    internal static class Util
    {
        internal static void PrintEntries(MessageCollection<MyCustomer> results)
        {
            Console.ForegroundColor = ConsoleColor.White;
            
            foreach (var result in results.Results)
            {
                Console.WriteLine("----------");
                Console.WriteLine(result);
            }

            Console.WriteLine("----------");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}
