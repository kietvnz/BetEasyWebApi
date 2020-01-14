using BetEasy.Contracts.Configurations;
using BetEasy.Contracts.Interfaces.Services;
using BetEasy.Contracts.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetEasy.Services
{
    public class HorseService : IHorseService
    {
        private readonly IOptions<AppConfig> _appConfig;
        private readonly ILogger<HorseService> _logger;
        private readonly IJsonFileParser _jsonFileParser;
        private readonly IXmlFileParser _xmlFileParser;

        public HorseService(IOptions<AppConfig> appConfig, ILogger<HorseService> logger, IJsonFileParser jsonFileParser, IXmlFileParser xmlFileParser)
        {
            _appConfig = appConfig;
            _logger = logger;
            _jsonFileParser = jsonFileParser;
            _xmlFileParser = xmlFileParser;
        }

        public async Task<IEnumerable<Horse>> GetHorsesWithPrice()
        {
            try
            {
                var xmlHorses = await _jsonFileParser.ParseAsync();
                var jsonHorses = await _xmlFileParser.ParseAsync();

                return xmlHorses.Concat(jsonHorses).OrderBy(h => h.Price);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while parsing feed data from Json and Xml files: {ex.Message}");
            }

            return null;
        }
    }
}
