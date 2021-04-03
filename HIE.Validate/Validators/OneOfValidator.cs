using HIE.Validate.Models;
using System;
using System.Linq;

namespace HIE.Validate.Validators
{
    class OneOfValidator : IStringValidator
    {
        private readonly string[] _allowedValues;

        public OneOfValidator(string[] allowedValues)
        {
            _allowedValues = allowedValues;
        }
        public ValidationResult Validate(string value)
        {
            return _allowedValues.Contains(value)
                ? new ValidationResult { IsValid = true, Message = $"{value} is one of allowed values" }
                : new ValidationResult { IsValid = false, Message = $"{value} is not one of; {string.Join(", ", _allowedValues)}" };
        }
    }
}
