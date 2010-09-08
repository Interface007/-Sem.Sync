namespace Sem.GenericHelpers.Contracts
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class ContractRuleAttribute : Attribute
    {
        public ContractRuleAttribute(Type type, string methodName)
        {
            this.Type = type;
            this.MethodName = methodName;
        }

        public Type Type { get; set; }

        public string MethodName { get; set; }
    }
}
