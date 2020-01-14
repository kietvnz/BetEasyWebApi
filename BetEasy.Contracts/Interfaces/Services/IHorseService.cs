using BetEasy.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetEasy.Contracts.Interfaces.Services
{
    public interface IHorseService
    {
        Task<IEnumerable<Horse>> GetHorsesWithPrice();
    }
}
