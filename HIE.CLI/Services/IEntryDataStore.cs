using HIE.CLI.Records;

namespace HIE.CLI.Services
{
    public interface IEntryDataStore
    {
        InventoryEntry[] GetInvalidEntries();
        InventoryEntry[] GetValidEntries();
        void SetInvalidEntries(InventoryEntry[] entries);
        void SetValidEntries(InventoryEntry[] entries);
    }
}