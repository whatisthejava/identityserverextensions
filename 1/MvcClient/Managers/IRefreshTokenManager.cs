using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcClient.Managers
{
    public interface IRefreshTokenManager
    {
        Task<TokenResponse> RefreshToken(string refreshToken, List<string> extraScopes = null);
    }
}