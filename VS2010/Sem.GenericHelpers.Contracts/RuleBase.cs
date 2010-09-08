// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleBase.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;

    public abstract class RuleBase<TExpression> 
    {
        public string Message { get; set; }
        public Func<string, string, Exception> ThrowException { get; set; }
        public TExpression CheckExpression { get; set; }

        protected RuleBase()
        {
            this.Message = "There is a problem with the parameter.";
            this.ThrowException = (message, parameterName) => new ArgumentException(this.Message, parameterName);
        }

        protected void InvokeInternal(Func<bool> toCheck, string parameterName)
        {
            if (!toCheck())
            {
                var exception = this.ThrowException(this.Message, parameterName);
                throw exception;
            }
        }
    }
}