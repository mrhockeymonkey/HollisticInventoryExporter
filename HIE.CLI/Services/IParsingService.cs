using HIE.CLI.Records;
using System.Collections.Generic;

namespace HIE.CLI.Services
{
    public interface IParsingService
    {
        IEnumerable<InventoryEntry> ParseFile(string file);
    }
}