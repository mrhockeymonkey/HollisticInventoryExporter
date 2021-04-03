using HIE.Validate.Models;

namespace HIE.Validate.Validators
{
    class FqdnValidator : IStringValidator
    {
        private string _domainSuffix;

        public FqdnValidator(string domainSuffix)
        {
            _domainSuffix = domainSuffix;
        }

        public ValidationResult Validate(string value)
        {
            return value.EndsWith(_domainSuffix)
                ? new ValidationResult { IsValid = true, Message = $"{value} is fully qualified." }
                : new ValidationResult { IsValid = false, Message = $"{value} does not end with {_domainSuffix}." };
        }
    }
}
