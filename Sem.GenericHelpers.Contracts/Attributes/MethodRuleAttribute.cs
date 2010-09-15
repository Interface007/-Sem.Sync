namespace Sem.GenericHelpers.Contracts.Attributes
{
    using System;

    /// <summary>
    /// Attribute to attach rules to methods. To attach rules to classes and properties, 
    /// use the <see cref="ContractRuleAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MethodRuleAttribute : ContractRuleAttribute
    {
        public MethodRuleAttribute(Type type, String methodParameterName)
            : base(type)
        {
            this.MethodParameterName = methodParameterName;
        }

        public string MethodParameterName { get; set; }
    }
}