using System;

namespace Sem.Sample.Contracts
{
    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.SemRules;

    class Program
    {
        static void Main(string[] args)
        {
            TryCall(
                () => new MyBusinessComponent().CallCustomer(new MyCustomer()),
                "We'll call a method that relies on\n\n" +
                "customer.InternalId.ToString()\n\n" +
                "while InternalId is not set, this will cause an exception:");
            
            TryCall(
                () => new MySaveBusinessComponent().CallCustomer(new MySaveCustomer()),
                "This call will use a method including the statement\n\n" +
                "Bouncer.For(() => customer).Assert();\n\n" +
                "The bouncer will tell you the root of the error inside the message\n" +
                "of the exception and will throw the exception in that method that\n" +
                "already can detect the issue.");
            
            TryCall(
                () => new MySaveBusinessComponent().CallCustomer2(new MySaveCustomer()),
                "In this example we will print out a list of messages from the statement\n\n" +
                "var results = Bouncer.ForMessages(() => saveBusinessObject).Assert();\n\n" +
                "This does not throw an exception, but executes all checks and returns \n" +
                "a list of issues.");
        }

        private static void TryCall(Action y, string description)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.WriteLine(description);
            Console.WriteLine();

            try
            {
                y.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace.ToString().Substring(0, 512));
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("press enter to execute next sample...");
            Console.ReadLine();
        }
    }

    internal class MySaveCustomer : MyCustomer
    {
        [ContractRule(typeof(StringNotNullOrEmptyRule))]
        public new string FullName { get; set; }

        [ContractRule(typeof(ObjectNotNullRule<CustomerId>))]
        public new CustomerId InternalId { get; set; }
    }

    internal class MyCustomer
    {
        public string FullName { get; set; }
        public CustomerId InternalId { get; set; }
    }
}
