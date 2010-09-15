// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractRuleAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContractRuleAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Attributes
{
    using System;

    /// <summary>
    /// Attribute to attach rules to classes and properties of classes. To attach rules to method parameters, 
    /// use the <see cref="MethodRuleAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class ContractRuleAttribute : Attribute
    {
        public ContractRuleAttribute(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; set; }
        public object Parameter { get; set; }
        public string Namespace { get; set; }
        public string Message { get; set; }
    }
}
