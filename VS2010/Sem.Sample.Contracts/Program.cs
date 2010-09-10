using System;

namespace Sem.Sample.Contracts
{
    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.SemRules;

    class Program
    {
        static void Main(string[] args)
        {
            var businessObject = new MyCustomer();
            var saveBusinessObject = new MySaveCustomer();

            var businessComponen = new MyBusinessComponent();
            var saveBusinessComponen = new MySaveBusinessComponent();

            ////// this will show what error message you normally will get without any parameter checking
            ////TryCall(() => businessComponen.CallCustomer(businessObject));

            ////// this will show what error message you will get with using the rules
            ////TryCall(() => saveBusinessComponen.CallCustomer(saveBusinessObject));

            // this will show what messages you can get without event raise an exception
            TryCall(() => saveBusinessComponen.CallCustomer2(saveBusinessObject));

            Console.ReadLine();
        }

        private static void TryCall(Action y)
        {
            try
            {
                y.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine();
            }
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
