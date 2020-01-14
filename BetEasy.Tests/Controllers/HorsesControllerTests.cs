using BetEasy.Api.Controllers;
using BetEasy.Contracts.Configurations;
using BetEasy.Contracts.Interfaces.Services;
using BetEasy.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BetEasy.Tests
{
    public class HorsesControllerTests
    {
        [Fact]
        public async Task ShouldReturnAnOkResponseSuccessfully()
        {
            //Arrange
            var appConfig = new AppConfig { JsonFilePath = "FeedData\\Wolferhampton_Race1.json", XmlFilePath = "FeedData\\Caulfield_Race1.xml" };
            var IOptionsMock = Options.Create(appConfig);

            var loggerMock = new Mock<ILogger<HorsesController>>();

            var horseServiceMock = new Mock<IHorseService>();

            horseServiceMock.Setup(service => service.GetHorsesWithPrice()).ReturnsAsync(
               GetHorsesWithPrice()
           );

            var controller = new HorsesController(IOptionsMock, loggerMock.Object, horseServiceMock.Object);

            //Act 
            var result = await controller.GetHorsesWithPrice();
            var okResult = result.Result as OkObjectResult;

            IEnumerable<Horse> horses = (IEnumerable<Horse>)okResult.Value;
            Horse horse1 = ((IEnumerable<Horse>)okResult.Value).First();
            Horse horse2 = horses.Skip(1).First();

            //Assert
            Assert.True(horses.Count() == 4);
            Assert.True(horse1.Name == "Advancing"
                                      && horse1.Price == 4.2m);

            Assert.True(horse2.Name == "Fikhaar"
                                       && horse2.Price == 4.4m);
        }

        private IEnumerable<Horse> GetHorsesWithPrice()
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
            return horses.OrderBy(h => h.Price);
        }
    }
}
