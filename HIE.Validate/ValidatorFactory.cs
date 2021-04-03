using HIE.Validate.Models;
using HIE.Validate.Validators;
using System;
using Newtonsoft.Json.Linq;

namespace HIE.Validate
{
    public class ValidatorFactory : IValidatorFactory
    {
        public IStringValidator CreateValidator(ValidatorConfig config)
        {
            return config switch
            {
                ValidatorConfig { Validator: ValidatorType.Fqdn } fqdn when fqdn.Opt is string =>
                    new FqdnValidator((string)fqdn.Opt),
                ValidatorConfig { Validator: ValidatorType.MaxChars } maxChars when maxChars.Opt is long =>
                    new MaxCharsValidator((long)maxChars.Opt),
                ValidatorConfig { Validator: ValidatorType.OneOf } oneOf when oneOf.Opt is JArray =>
                    new OneOfValidator(((JArray)oneOf.Opt).ToObject<string[]>()),
                null => throw new ArgumentNullException(),
                _ => throw new ArgumentException($"Invalid config for {config.Validator}, Opt: '{config.Opt}'"),
            };
        }
    }
}
