namespace Sem.GenericHelpers.Contracts.Attributes
{
    using System;

    /// <summary>
    /// Attribute to attach rules to methods. To attach rules to classes and properties, 
    /// use the <see cref="ContractRuleAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class ContractContextAttribute : Attribute
    {
        public ContractContextAttribute(String contextName)
            :this(contextName, true)
        {
        }

        public ContractContextAttribute(String contextName, bool active)
        {
            this.Context = contextName;
            this.Active = active;
        }

        public string Context { get; set; }
        public bool Active { get; set; }
    }
}