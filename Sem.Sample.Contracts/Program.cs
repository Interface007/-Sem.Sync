﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sample.Contracts
{
    using System;

    using Sem.GenericHelpers.Contracts;
    using Sem.Sample.Contracts.Entities;

    public class Program
    {
        public static void Main(string[] args)
        {
            Util.TryCall(
                "We'll call a method that relies on\n\n" +
                "customer.InternalId.ToString()\n\n" +
                "while InternalId is not set, this will cause an exception (do\n" +
                "you see any hint, what property causes the exception?):",
                () => new MyBusinessComponent().WriteCustomerProperties(new MyCustomer { EMailAddress = "sven@svenerikmatzen.info" }));

            Util.TryCall(
                "This call will use a method including the statement\n\n" +
                "Bouncer.For(() => customer).Assert();\n\n" +
                "The bouncer will tell you the root of the error inside the message of the \n" +
                "exception and will throw the exception in the first method that can detect the \n" +
                "issue. Also it will give you the property name:",
                () => new MyBusinessComponentSave().WriteCustomerProperties(new MyCustomer { EMailAddress = "sven@svenerikmatzen.info" }));

            Util.TryCall(
                "In this example we will print out a list of messages from the statement\n\n" +
                "var results = Bouncer.ForMessages(() => saveBusinessObject).Assert();\n\n" +
                "This does not throw an exception, but executes all checks and returns \n" +
                "a list of issues.",
                () => new MyBusinessComponentSave().CheckCustomerProperties(new MyCustomer { EMailAddress = "sven@svenerikmatzen.info" }));

            AddLogging(
                "Rule executers do also support central inspection of rule evaluation results.\n" +
                "In our example we can activate logging to the screen by adding an System.Action\n" +
                "to the Bouncer.AfterInvokeAction list, that will simply log some information to\n" +
                "the console in ConsoleColor.Yellow.\n\n" +
                "You might wonder why you get more logs than rule violations: the logging will\n" +
                "get any validation result - even the successfully validated data.\n\n" +
                "Enter >>L<< to activate logging.");

            Util.TryCall(
                "Lets execute a completely new rule (\".ForMessages(() => customer)\" again).\n\n" +
                "Rule   : (x, y) => x.FullName != \"Sven\"\n" +
                "Message: \"Sven cannot enter this method\"\n\n" +
                "This does not throw an exception, but executes all checks and returns \n" +
                "a list of issues.",
                () => new MyBusinessComponentSave().CheckCustomerWithCustomRule(new MyCustomer { FullName = "Sven" }));

            Util.TryCall(
                "We'll call a method containing a MethodRuleAttribute that defines a\n" +
                "rule for its parameter. This way we nearly completely are declarative\n" +
                "unfortunately we cannot enforce the parameter name to match to the method\n" +
                "signature at compile time.",
                () => new MyBusinessComponentSave().CheckCustomerWithWithMethodAttributes(string.Empty, 1000, new MyCustomer { EMailAddress = "sven@svenerikmatzen.info" }));

            Util.TryCall(
                "We also can use a string as a context description for the call. E.g. we can\n" +
                "tell the engine that we currently are creating new data, so we cannot set the\n" +
                "internal id (this will be generated by the database while inserting the data).\n" +
                "In this case this 'context' will activate some rules and deactivate others.\n" +
                "In our case, one rule for InternalId will be replaced with the exact opposit:\n" +
                "for InternalId => 'IsNullRule' instead of 'IsNotNullRule'.\n" +
                "The rules for specifying the full name and a valid email address (the string\n" +
                "\"don't@spam.me\" does contain a '-char) are still active.",
                () => new MyBusinessComponentSave().InsertCustomer(new MyCustomer { FullName = "Sven", EMailAddress = "don't@spam.me"}));
        }

        private static void AddLogging(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.WriteLine(message);

            var input = Console.ReadLine();
            if (input == null)
            {
                return;
            }

            if (input.ToUpperInvariant() == "L")
            {
                Bouncer.AfterInvokeAction.Add(x =>
                    {
                        var c = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("LOG: " + x.RuleType.Name + " => " + x.Result + " for " + x.ValueName + "\n" + x.Message);
                        Console.ForegroundColor = c;
                    });
            }
        }
    }
}
