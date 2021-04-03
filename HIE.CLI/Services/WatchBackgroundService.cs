using HIE.CLI.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HIE.CLI.Services
{
    /// <summary>
    /// Takes care of constantly parsing, validating and adding entries to the data store
    /// </summary>
    public class WatchBackgroundService : BackgroundService
    {
        private readonly IInventoryDataValidator _validator;
        private readonly IParsingService _parser;
        private readonly IEntryDataStore _store;
        private readonly HieSettings _settings;

        public WatchBackgroundService(
            IInventoryDataValidator validator, 
            IParsingService parser,
            IEntryDataStore store,
            IOptions<HieSettings> settings)
        {
            _validator = validator;
            _parser = parser;
            _store = store;
            _settings = settings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            Console.WriteLine($"watching {_settings.ReadFromFile} ");

            while (true)
            {
                try
                {
                    var data = _parser.ParseFile(_settings.ReadFromFile);
                    Console.WriteLine($"Successfully parsed {_settings.ReadFromFile} into {data.Count()} items");

                    var lookup = data.ToLookup(
                        entry => _validator.Validate(entry),
                        entry => entry);
                    
                    var valid = lookup[true].ToArray();
                    var invalid = lookup[false].ToArray();

                    _store.SetValidEntries(valid);
                    _store.SetInvalidEntries(invalid);
                    Console.WriteLine($"WARNING: {invalid.Length} invalid entries skipped");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to parse {_settings.ReadFromFile}. {e.Message}");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            }
        }
    }
}
