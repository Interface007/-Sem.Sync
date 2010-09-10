// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectNotNullRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ObjectNotNullRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.SemRules
{
    public class ObjectNotNullRule<TData> : RuleBase<TData, object>
        where TData : class
    {
        public ObjectNotNullRule()
        {
            this.CheckExpression = (target, parameter) => target != null;
            this.Message = "The object is NULL.";
        }
    }
}