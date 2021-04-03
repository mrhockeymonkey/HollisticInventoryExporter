using HIE.CLI.Configuration;
using HIE.CLI.Records;
using HIE.Validate;
using HIE.Validate.Models;
using System.Collections.Generic;
using System.Linq;

namespace HIE.CLI
{
    /// <summary>
    /// Responsible for applying validations to supplied inventory data
    /// </summary>
    public class InventoryDataValidator : IInventoryDataValidator
    {
        private readonly ValidationConfig _config;
        private readonly IValidatorFactory _factory;
        public InventoryDataValidator(ValidationConfig config, IValidatorFactory factory) =>
            (_config, _factory) = (config, factory);

        public bool Validate(InventoryEntry entry) => GetValidationResults(entry).All(r => r.IsValid);

        public IEnumerable<ValidationResult> GetValidationResults(InventoryEntry entry)
        {
            List<ValidationResult> results = new();
            results.AddRange(GetHostnameResults(entry));
            results.AddRange(GetOperatingSystemResults(entry));

            return results;
        }

        private IEnumerable<ValidationResult> GetHostnameResults(InventoryEntry entry) =>
            _config.Hostname.Select(config => _factory.CreateValidator(config).Validate(entry.Hostname));

        private IEnumerable<ValidationResult> GetOperatingSystemResults(InventoryEntry entry) =>
            _config.OperatingSystem.Select(config => _factory.CreateValidator(config).Validate(entry.OperatingSystem));

    }
}
