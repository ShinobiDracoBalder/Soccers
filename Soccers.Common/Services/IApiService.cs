using Soccers.Common.Models;
using System.Threading.Tasks;

namespace Soccers.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
    }
}
