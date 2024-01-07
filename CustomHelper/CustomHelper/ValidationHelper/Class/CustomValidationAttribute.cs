using CustomHelper.ValidationHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.ValidationHelper.Class
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class CustomValidationAttribute : ValidationAttribute, IValidationRule
    {
        private readonly IValidationRule validationRule;

        /// <summary>
        /// To create custom validation atribots, you need to specify the type of validation that implements the IValidationRule interface
        /// Then pass the parameters needed for validation
        /// Example: [CustomValidation(typeof(DisallowValuesRule), "string", "int", "decimal")]
        /// </summary>
        /// <param name="ruleType"></param>
        /// <param name="ruleParameters"></param>
        /// <exception cref="ArgumentException"></exception>
        public CustomValidationAttribute(Type ruleType, params object[] ruleParameters)
        {
            if (typeof(IValidationRule).IsAssignableFrom(ruleType))
            {
                validationRule = (IValidationRule)Activator.CreateInstance(ruleType, ruleParameters);
            }
            else
            {
                throw new ArgumentException("The ruleType must implement IValidationRule.");
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationRule.IsValid(value))
            {
                return ValidationResult.Success;
            }
            else
            {
                if (double.TryParse(value.ToString(), out _))
                {
                    value.ToString();
                }

                return new ValidationResult($"The value '{value}' is not allowed for this property.");
            }
        }
    }
}
