using HIE.Validate.Models;

namespace HIE.Validate.Validators
{
    class MaxCharsValidator : IStringValidator
    {
        private long _maxChars;

        public MaxCharsValidator(long maxChars)
        {
            _maxChars = maxChars;
        }

        public ValidationResult Validate(string value)
        {
            return value.Length <= _maxChars
                ? new ValidationResult { IsValid = true, Message = $"{value} is less than or equal to {_maxChars} characters." }
                : new ValidationResult { IsValid = false, Message = $"{value} has more that {_maxChars} characters." };
        }
    }
}
