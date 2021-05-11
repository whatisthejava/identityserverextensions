using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MvcClient.Managers
{
    public interface IExternalGrantManager
    {
        Task<string> ExecuteDelegation(HttpContext context);
    }
}