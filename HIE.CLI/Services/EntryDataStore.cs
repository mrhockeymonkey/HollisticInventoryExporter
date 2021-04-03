using HIE.CLI.Records;
using System.Threading;

namespace HIE.CLI.Services
{
    public class EntryDataStore : IEntryDataStore
    {
        private InventoryEntry[] _validEntries;
        private InventoryEntry[] _invalidEntries;
        private ReaderWriterLockSlim validLock;
        private ReaderWriterLockSlim invalidLock;

        public EntryDataStore()
        {
            _validEntries = new InventoryEntry[0];
            _invalidEntries = new InventoryEntry[0];
        }

        public InventoryEntry[] GetValidEntries()
        {
            return _validEntries;
        }

        public void SetValidEntries(InventoryEntry[] entries)
        {
            _validEntries = entries;
        }

        public InventoryEntry[] GetInvalidEntries()
        {
            return _invalidEntries;
        }

        public void SetInvalidEntries(InventoryEntry[] entries)
        {
            _invalidEntries = entries;
        }
    }
}
