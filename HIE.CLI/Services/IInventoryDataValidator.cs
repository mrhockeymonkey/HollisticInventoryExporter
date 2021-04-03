using HIE.CLI.Records;
using HIE.Validate.Models;
using System.Collections.Generic;

namespace HIE.CLI
{
    public interface IInventoryDataValidator
    {
        IEnumerable<ValidationResult> GetValidationResults(InventoryEntry entry);
        bool Validate(InventoryEntry entry);
    }
}