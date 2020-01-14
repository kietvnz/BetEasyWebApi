using BetEasy.Contracts.Configurations;
using BetEasy.Contracts.Interfaces.Services;
using BetEasy.Contracts.Models;
using BetEasy.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BetEasy.Tests.Services
{
    public class HorseServiceTests
    {
        [Fact]
        public async Task ShouldGetAllHorseInformationInPriceAscendingOrder()
        {
            //Arrange
            var appConfig = new AppConfig { JsonFilePath = "FeedData\\Wolferhampton_Race1.json", XmlFilePath = "FeedData\\Caulfield_Race1.xml" };
            var IOptionsMock = Options.Create(appConfig);

            var loggerMock = new Mock<ILogger<HorseService>>();

            var jsonFileParserMock = new Mock<IJsonFileParser>();
            jsonFileParserMock.Setup(service => service.ParseAsync()).ReturnsAsync(
                 GetHorsesFromJsonFile()
             );

            var xmlFileParserMock = new Mock<IXmlFileParser>();
            xmlFileParserMock.Setup(service => service.ParseAsync()).ReturnsAsync(
                 GetHorsesFromXmlFile()
             );

            var horseService = new HorseService(IOptionsMock, loggerMock.Object, jsonFileParserMock.Object, xmlFileParserMock.Object);
            //Act 
            var horses = await horseService.GetHorsesWithPrice();
            Horse horse1 = horses.First();
            Horse horse2 = horses.Skip(1).First();
            Horse horse3 = horses.Skip(2).First();
            Horse horse4 = horses.Skip(3).First();

            //Assert
            Assert.True(horses.Count() == 4);
            Assert.True(horse1.Name == "Advancing"
                                      && horse1.Price == 4.2m);

            Assert.True(horse2.Name == "Fikhaar"
                                       && horse2.Price == 4.4m);

            Assert.True(horse3.Name == "Toolatetodelegate"
                                       && horse3.Price == 10m);

            Assert.True(horse4.Name == "Coronel"
                                       && horse4.Price == 12m);
        }

        private IEnumerable<Horse> GetHorsesFromJsonFile()
        {
            var horses = new List<Horse>();
            horses.Add(new Horse()
            {
                Name = "Toolatetodelegate",
                Price = 10m
            });

            horses.Add(new Horse()
            {
                Name = "Fikhaar",
                Price = 4.4m
            });

            return horses;
        }

        private IEnumerable<Horse> GetHorsesFromXmlFile()
        {
            var horses = new List<Horse>();

            horses.Add(new Horse()
            {
                Name = "Advancing",
                Price = 4.2m
            });

            horses.Add(new Horse()
            {
                Name = "Coronel",
                Price = 12m
            });
            return horses;
        }
    }
}
