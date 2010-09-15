// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyBusinessComponentSave.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MyBusinessComponentSave type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sample.Contracts.Entities
{
    using System;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rules;

    internal class MyBusinessComponentSave : MyBusinessComponent
    {
        /// <summary>
        /// We need to use the type MySaveCustomer in order to correctly resolve type inference for  
        /// Bouncer.ForCheckData(() => customer).Assert();
        /// </summary>
        /// <param name="customer">this customer type does have rule-attributes attached to its properties</param>
        internal new void CallCustomer(MyCustomer customer)
        {
            Bouncer.ForCheckData(() => customer).Assert();

            Console.WriteLine(
                "calling customer {0} with Id {1}", 
                GetTheName(customer), 
                FormatTheId(customer));
        }

        /// <summary>
        /// This time we will not prevent executing the method code, but 
        /// just collect the violated rules and print them to the console.
        /// </summary>
        /// <param name="customer"></param>
        internal void CallCustomer2(MyCustomer customer)
        {
            var results = Bouncer.ForMessages(() => customer).Assert();
            Util.PrintEntries(results);
        }

        /// <summary>
        /// This time we will not prevent executing the method code, but 
        /// just collect the violated rules and print them to the console.
        /// </summary>
        /// <param name="customer"></param>
        internal void CallCustomerWithCustomRule(MyCustomer customer)
        {
            var results = Bouncer
                .ForMessages(() => customer)
                .Assert(new RuleBase<MyCustomer, object>
                    {
                        Message = "Sven cannot enter this method",
                        CheckExpression = (x, y) => x.FullName != "Sven"
                    });

            Util.PrintEntries(results);
            Console.WriteLine("---> ForMessages did return the validation results, but  <---");
            Console.WriteLine("--->   did not cause any exception. So \"customer Sven\"   <---");
            Console.WriteLine("--->   did enter the method  and did execute all code.   <---");
        }
        
        [MethodRule(typeof(IntegerLowerThanRule), "amount", Parameter = 100)]
        [MethodRuleAttribute(typeof(StringNotNullOrEmptyRule), "customerId")]
        public void CallCustomerWithMethodAttributes(string customerId, int amount, MyCustomer theCustomer)
        {
            var results = Bouncer
                .ForMessages(() => amount)
                .ForMessages(() => theCustomer)
                .Assert();

            Util.PrintEntries(results);
        }
    }
}