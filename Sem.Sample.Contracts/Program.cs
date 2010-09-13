using System;

namespace Sem.Sample.Contracts
{
    class Program
    {
        static void Main(string[] args)
        {
            TryCall("We'll call a method that relies on\n\n" +
                    "customer.InternalId.ToString()\n\n" +
                    "while InternalId is not set, this will cause an exception (do\n" +
                    "you see any hint, what property causes the exception?):",
                    () => new MyBusinessComponent().CallCustomer(new MyCustomer()));

            TryCall("This call will use a method including the statement\n\n" +
                    "Bouncer.For(() => customer).Assert();\n\n" +
                    "The bouncer will tell you the root of the error inside the message of the \n" +
                    "exception and will throw the exception in the first method that can detect the \n" +
                    "issue. Also it will give you the property name:",
                    () => new MyBusinessComponentSave().CallCustomer(new MyCustomer()));

            TryCall("In this example we will print out a list of messages from the statement\n\n" +
                    "var results = Bouncer.ForMessages(() => saveBusinessObject).Assert();\n\n" +
                    "This does not throw an exception, but executes all checks and returns \n" +
                    "a list of issues.",
                    () => new MyBusinessComponentSave().CallCustomer2(new MyCustomer()));

            TryCall("Lets execute a completely new rule.\n\n" +
                    "Rule   : (x, y) => x.FullName != \"Sven\"\n" +
                    "Message: \"Sven cannot enter this method\"\n\n" +
                    "This does not throw an exception, but executes all checks and returns \n" +
                    "a list of issues.",
                    () => new MyBusinessComponentSave().CallCustomerWithCustomRule(new MyCustomer { FullName = "Sven" }));
        }

        private static void TryCall(string description, Action y)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.WriteLine(description);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
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
                Console.WriteLine(ex.StackTrace.Substring(0, 300));
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("press enter to execute next sample...");
            Console.ReadLine();
        }
    }
}
