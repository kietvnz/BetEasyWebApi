using BetEasy.Contracts.Configurations;
using BetEasy.Contracts.Interfaces.Services;
using BetEasy.Contracts.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BetEasy.Services
{
    public class JsonFileParser : IJsonFileParser
    {
        private readonly IOptions<AppConfig> _appConfig;
        private readonly ILogger<JsonFileParser> _logger;

        public JsonFileParser(IOptions<AppConfig> appConfig, ILogger<JsonFileParser> logger)
        {
            _appConfig = appConfig;
            _logger = logger;
        }
        public async Task<IEnumerable<Horse>> ParseAsync()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appConfig.Value.JsonFilePath);
            using (StreamReader sr = new StreamReader(filePath))
            {
                string text = await sr.ReadToEndAsync();

                JObject feed = JObject.Parse(text);
                IEnumerable<dynamic> selections = feed.SelectTokens("..Selections[*]");

                return selections.Select(selection => new Horse { Name = selection.Tags.name, Price = selection.Price });
            }
        }
    }
}
