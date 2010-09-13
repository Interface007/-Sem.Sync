// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationValidatorBaseRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ConfigurationValidatorBaseRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.SemRules
{
    using System;
    using System.Configuration;

    public class ConfigurationValidatorBaseRule<TData>: RuleBase<TData, object>
    {
        public ConfigurationValidatorBase ConfigurationValidator { get; set; }

        public ConfigurationValidatorBaseRule(ConfigurationValidatorBase validator)
        {
            this.ConfigurationValidator = validator;
        }

        public ConfigurationValidatorBaseRule()
        {
            this.CheckExpression = CheckExpression = (data, parameter) =>
                {
                    try
                    {
                        this.ConfigurationValidator.Validate(data);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                };

            var type = this.ConfigurationValidator.GetType();
            this.Message = string.Format("The validator {0} did throw an exception.", type.Namespace + "." + type.Name);
        }
    }
}
