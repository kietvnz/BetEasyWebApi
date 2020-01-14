using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetEasy.Contracts.Interfaces.Services
{
    public interface IFileParser<T> where T : class
    {
        Task<IEnumerable<T>> ParseAsync();
    }
}
