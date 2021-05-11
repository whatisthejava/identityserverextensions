using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MvcClient.Managers
{
    public interface ICallExternalApiManager
    {
        Task<string> CallApi(string accessToken, string url);
    }
}