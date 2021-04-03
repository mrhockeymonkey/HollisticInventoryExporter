using HIE.Validate.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HIE.Validate.Models
{
    public class ValidatorConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ValidatorType Validator { get; set; }
        public object Opt { get; set; }
    }
}