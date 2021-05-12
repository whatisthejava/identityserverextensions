using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MvcClient.Managers
{
    public interface IRefreshTokenManager
    {
        Task<TokenResponse> RefreshToken(string token);
    }
}