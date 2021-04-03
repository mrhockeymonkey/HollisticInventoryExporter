using Newtonsoft.Json;
using System;

namespace HIE.CLI.Configuration
{
    /// <summary>
    /// Represents the inventory owner settings that can be customized 
    /// with their desired/relevant validations
    /// </summary>
    public class HieSettings
    {
        [JsonProperty(Required = Required.Always)]
        public string InventoryName { get; set; } = "Undefined";
        public string ReadFromFile { get; set; } = String.Empty;
        public ValidationConfig Validation { get; set; }
    }
}
