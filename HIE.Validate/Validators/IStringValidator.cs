using HIE.Validate.Models;

namespace HIE.Validate.Validators
{
    public interface IStringValidator
    {
        public ValidationResult Validate(string value);
    }
}
