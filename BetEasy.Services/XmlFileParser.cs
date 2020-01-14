using BetEasy.Contracts.Configurations;
using BetEasy.Contracts.Interfaces.Services;
using BetEasy.Contracts.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BetEasy.Services
{
    public class XmlFileParser : IXmlFileParser
    {
        private readonly IOptions<AppConfig> _appConfig;
        private readonly ILogger<XmlFileParser> _logger;

        public XmlFileParser(IOptions<AppConfig> appConfig, ILogger<XmlFileParser> logger)
        {
            _appConfig = appConfig;
            _logger = logger;
        }
        public async Task<IEnumerable<Horse>> ParseAsync()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appConfig.Value.XmlFilePath);
            using (StreamReader sr = new StreamReader(filePath))
            {
                string text = await sr.ReadToEndAsync();

                XDocument xmlDoc = XDocument.Parse(text);
                var prices = xmlDoc.Descendants("race").Elements("prices").Single().Descendants("horse");
                var horses = xmlDoc.Descendants("race").Elements("horses").Single().Descendants("horse");

                return horses.Select(h =>
                        new Horse
                        {
                            Name = h.Attribute("name").Value,
                            Price = decimal.Parse(prices.Single(p => p.Attribute("number").Value == h.Element("number").Value).Attribute("Price").Value)
                        }
                    );
            }
        }
    }
}