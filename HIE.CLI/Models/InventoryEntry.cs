using Newtonsoft.Json;

namespace HIE.CLI.Records
{
    public class InventoryEntry
    {
        [JsonProperty(Required = Required.Always)]
        public string Hostname;

        [JsonProperty(Required = Required.Always)]
        public string OperatingSystem;

        public override string ToString() =>
            $"InventoryData: {nameof(Hostname)}='{Hostname}', {nameof(OperatingSystem)}='{OperatingSystem}'";

        public InventoryEntryDto ToDto() => new InventoryEntryDto{
            Hostname = Hostname,
            OperatingSystem = OperatingSystem,
        };
    }
}
