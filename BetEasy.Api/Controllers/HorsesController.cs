using BetEasy.Contracts.Configurations;
using BetEasy.Contracts.Interfaces.Services;
using BetEasy.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetEasy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorsesController : ControllerBase
    {
        private readonly IOptions<AppConfig> _appConfig;
        private readonly ILogger<HorsesController> _logger;
        private readonly IHorseService _horseService;

        public HorsesController(IOptions<AppConfig> appConfig, ILogger<HorsesController> logger, IHorseService horseSerivce)
        {
            _appConfig = appConfig;
            _logger = logger;
            _horseService = horseSerivce;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Horse>>> GetHorsesWithPrice()
        {
            return Ok(await _horseService.GetHorsesWithPrice());
        }
    }
}
