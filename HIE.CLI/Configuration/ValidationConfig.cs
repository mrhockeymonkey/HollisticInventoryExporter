using HIE.Validate.Models;
using System.Collections.Generic;

namespace HIE.CLI.Configuration
{
    public class ValidationConfig
    {
        public IEnumerable<ValidatorConfig> Hostname { get; set; }
        public IEnumerable<ValidatorConfig> OperatingSystem { get; set; }
    }
}