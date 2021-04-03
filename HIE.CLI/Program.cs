using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using HIE.CLI.Records;
using HIE.CLI.Services;
using HIE.CLI.Configuration;
using HIE.Validate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace HIE.CLI
{
    class Program
    {
        private static Rune failIcon = new Rune(0x1F4CC); // 📌
        private static Rune passIcon = new Rune(0x1F44C); // 👌

        private static Rune GetIcon(bool isValid)
        {
            return isValid
                ? passIcon
                : failIcon;
        }

        private static HieSettings GetSettings()
        {
            var parser = new NewtonsoftParser();
            var file = "appsettings.json";
            return parser.ParseSettings(file);
        }

        private static IEnumerable<InventoryEntry> GetData(string file)
        {
            var parser = new NewtonsoftParser();

            try
            {
                var data = parser.ParseFile(file);
                Console.WriteLine($"Successfully parsed {file} into {data.Count()} items");
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to parse {file}. {e.Message}");
                Environment.Exit(1);
                throw;
            }
        }

        static void Main(string[] args)
        {
            // define commands
            var rootCmd = new RootCommand("root");
            var parseCmd = new Command("parse");
            var validateCmd = new Command("validate");
            var serveCmd = new Command("serve");
            var configCmd = new Command("config");

            // setup validate command
            validateCmd.AddOption(new Option<string>("--file", "file"));
            validateCmd.AddOption(new Option<bool>("--all", "all"));
            validateCmd.Handler = CommandHandler.Create<string, bool>((file, all) =>
            {
                var settings = GetSettings();
                var data = GetData(file);
                ValidatorFactory factory = new();
                InventoryDataValidator validator = new(settings.Validation, factory);
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                foreach (var entry in data)
                {
                    Console.WriteLine($"Validating {entry.Hostname}");
                    var results = validator.GetValidationResults(entry).ToList();

                    if (all)
                    {
                        results.ForEach(r => Console.WriteLine($"{GetIcon(r.IsValid)} {r.Message}"));
                    }
                    else
                    {
                        results
                            .Where(r => !r.IsValid)
                            .ToList()
                            .ForEach(r => Console.WriteLine($"{failIcon} {r.Message}"));
                    }
                }
            });

            // setup parse command
            parseCmd.AddOption(new Option<string>("--file", "file"));
            parseCmd.Handler = CommandHandler.Create<string>((file) =>
            {
                var data = GetData(file);
            });

            // setup config command
            configCmd.Handler = CommandHandler.Create(() =>
            {
                try
                {
                    var settings = GetSettings();
                    Console.WriteLine($"Inventory Name: {settings.InventoryName}");
                    Console.WriteLine($"Hostname Validation: {settings.Validation.Hostname.Select(v => v.Validator).ToArray()}");

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to parse app settings. {e.Message}");
                }
            });

            // setup serve command
            serveCmd.AddOption(new Option<string>("--file", "file"));
            serveCmd.Handler = CommandHandler.Create<string>((file) =>
            {
                HieSettings settings = GetSettings();
                if (file != null)
                {
                    settings.ReadFromFile = file;
                }
                var data = GetData(settings.ReadFromFile);
                ValidatorFactory factory = new();
                InventoryDataValidator validator = new(settings.Validation, factory);
                EntryDataStore entryStore = new();

                // prepopulate data store before hosting
                var lookup = data.ToLookup(
                    entry => validator.Validate(entry),
                    entry => entry);

                var valid = lookup[true].ToArray();
                var invalid = lookup[false].ToArray();
                entryStore.SetValidEntries(valid);
                entryStore.SetInvalidEntries(invalid);

                Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseKestrel();
                        webBuilder.ConfigureServices(services =>
                        {
                            services.AddOptions<HieSettings>().Configure(opts =>
                            {
                                // is there a better way to do this? probably...
                                opts.InventoryName = settings.InventoryName;
                                opts.ReadFromFile = settings.ReadFromFile;
                                opts.Validation = settings.Validation;
                            });
                            services.AddControllers();
                            services.AddSingleton<IParsingService>(new NewtonsoftParser());
                            services.AddSingleton<IInventoryDataValidator>(validator);
                            services.AddSingleton<IEntryDataStore>(entryStore);
                            services.AddHostedService<WatchBackgroundService>();
                        });
                        webBuilder.Configure(app =>
                        {
                            app.UseRouting();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapControllers();
                            });
                        });
                    })
                    .Build()
                    .Run();
            });

            // setup command hierarchy
            rootCmd.AddCommand(validateCmd);
            rootCmd.AddCommand(parseCmd);
            rootCmd.AddCommand(configCmd);
            rootCmd.AddCommand(serveCmd);

            //rootCmd.Invoke("config");
            //rootCmd.InvokeAsync("validate --file data.json --all");
            //rootCmd.InvokeAsync("parse --file data.json").Wait();
            //rootCmd.InvokeAsync("serve --file data.json");

            rootCmd.InvokeAsync(args).Wait();
        }
    }
}
