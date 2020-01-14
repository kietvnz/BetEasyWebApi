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
    public class XmlFileParserTests
    {
        [Fact]
        public async Task ShouldParseJsonFileSuccessfully()
        {
            //Arrange
            var appConfig = new AppConfig { JsonFilePath = "FeedData\\Wolferhampton_Race1.json", XmlFilePath = "FeedData\\Caulfield_Race1.xml" };
            var IOptionsMock = Options.Create(appConfig);

            var loggerMock = new Mock<ILogger<XmlFileParser>>();

            var xmlFileParser = new XmlFileParser(IOptionsMock, loggerMock.Object);

            //Act 
            var horses = await xmlFileParser.ParseAsync();
            Horse horse1 = horses.First();
            Horse horse2 = horses.Skip(1).First();

            //Assert
            Assert.True(horses.Count() == 2);
            Assert.True(horse1.Name == "Advancing"
                                      && horse1.Price == 4.2m);

            Assert.True(horse2.Name == "Coronel"
                                       && horse2.Price == 12m);
        }

        [Fact]
        public async Task ShouldParseXmlFileWithAnException()
        {
            //Arrange
            var appConfig = new AppConfig { JsonFilePath = "", XmlFilePath = "" };
            var IOptionsMock = Options.Create(appConfig);

            var loggerMock = new Mock<ILogger<XmlFileParser>>();

            var xmlFileParser = new XmlFileParser(IOptionsMock, loggerMock.Object);

            //Act and Assert
            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => xmlFileParser.ParseAsync());
        }
    }
}
