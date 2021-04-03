using HIE.CLI.Records;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

using HIE.CLI.Configuration;

namespace HIE.CLI.Services
{
    class NewtonsoftParser : IParsingService
    {
        // TODO TryParseFile()

        public IEnumerable<InventoryEntry> ParseFile(string file)
        {
            var filePath = Path.GetFullPath(file);
            Console.WriteLine($"Parsing {filePath}");

            var json = File.ReadAllText(filePath);
            //Console.WriteLine(json);

            return JsonConvert.DeserializeObject<IEnumerable<InventoryEntry>>(json);
        }

        public HieSettings ParseSettings(string file)
        {
            var filePath = Path.GetFullPath(file);
            Console.WriteLine($"Parsing {filePath}");

            var json = File.ReadAllText(filePath);
            //Console.WriteLine(json);

            var settings = JsonConvert.DeserializeObject<HieSettings>(json);
            return settings;
        }
    }
}
