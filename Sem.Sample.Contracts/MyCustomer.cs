namespace Sem.Sample.Contracts
{
    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.SemRules;

    /// <summary>
    /// This class inherits from MyCustomer in order to be able to 
    /// call the exact same code for "GetTheName" and "FormatTheId".
    /// </summary>
    internal class MyCustomer //: MyCustomer
    {
        [ContractRule(typeof(ObjectNotNullRule<CustomerId>))]
        public CustomerId InternalId { get; set; }

        // This property will be checked to not be null or empty. Additionally we
        // alter the message to the message string specified inside the parameter
        [ContractRule(typeof(StringNotNullOrEmptyRule), Message = "You need to set the value of the property {1}.")]
        public string FullName { get; set; }
    }
}