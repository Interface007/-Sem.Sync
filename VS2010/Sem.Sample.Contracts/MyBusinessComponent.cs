// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyBusinessComponent.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MyBusinessComponent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sample.Contracts
{
    using System;
    using Sem.GenericHelpers.Contracts;

    internal class MyBusinessComponent
    {
        internal void CallCustomer(MyCustomer customer)
        {
            Console.WriteLine(
                "calling customer {0} with Id {1}", 
                GetTheName(customer), 
                FormatTheId(customer));
        }

        protected static string FormatTheId(MyCustomer customer)
        {
            return ">>" + customer.InternalId.ToString().Replace("-", string.Empty) + "<<";
        }

        protected static string GetTheName(MyCustomer customer)
        {
            return customer.FullName.Replace("-", " ");
        }
    }

    internal class MySaveBusinessComponent : MyBusinessComponent
    {
        internal void CallCustomer(MySaveCustomer customer)
        {
            Bouncer.ForCheckData(() => customer).Assert();

            Console.WriteLine(
                "calling customer {0} with Id {1}", 
                GetTheName(customer), 
                FormatTheId(customer));
        }

        internal void CallCustomer2(MySaveCustomer saveBusinessObject)
        {
            var results = Bouncer.ForMessages(() => saveBusinessObject).Assert();
            foreach (var result in results.Results)
            {
                Console.WriteLine(result);
            }
        }
    }
}
