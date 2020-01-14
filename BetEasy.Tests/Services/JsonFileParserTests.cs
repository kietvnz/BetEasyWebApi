using BetEasy.Contracts.Configurations;
using BetEasy.Contracts.Models;
using BetEasy.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BetEasy.Tests.Services
{
    public class JsonFileParserTests
    {
        [Fact]
        public async Task ShouldParseJsonFileSuccessfully()
        {
            //Arrange
            var appConfig = new AppConfig { JsonFilePath = "FeedData\\Wolferhampton_Race1.json", XmlFilePath = "FeedData\\Caulfield_Race1.xml" };
            var IOptionsMock = Options.Create(appConfig);

            var loggerMock = new Mock<ILogger<JsonFileParser>>();

            var jsonFileParser = new JsonFileParser(IOptionsMock, loggerMock.Object);

            //Act 
            var horses = await jsonFileParser.ParseAsync();
            Horse horse1 = horses.First();
            Horse horse2 = horses.Skip(1).First();

            //Assert
            Assert.True(horses.Count() == 2);
            Assert.True(horse1.Name == "Toolatetodelegate"
                                      && horse1.Price == 10m);

            Assert.True(horse2.Name == "Fikhaar"
                                       && horse2.Price == 4.4m);
        }

        [Fact]
        public async Task ShouldParseJsonFileWithAnException()
        {
            //Arrange
            var appConfig = new AppConfig { JsonFilePath = "", XmlFilePath = "FeedData\\Caulfield_Race1.xml" };
            var IOptionsMock = Options.Create(appConfig);

            var loggerMock = new Mock<ILogger<JsonFileParser>>();

            var jsonFileParser = new JsonFileParser(IOptionsMock, loggerMock.Object);

            //Act and Assert
            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => jsonFileParser.ParseAsync());
        }
    }
}
