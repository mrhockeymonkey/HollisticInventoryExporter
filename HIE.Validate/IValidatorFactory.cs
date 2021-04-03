using HIE.Validate.Models;
using HIE.Validate.Validators;

namespace HIE.Validate
{
    public interface IValidatorFactory
    {
        IStringValidator CreateValidator(ValidatorConfig config);
    }
}