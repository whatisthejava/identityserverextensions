using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MvcClient.Managers
{
    public class RefreshTokenManager : IRefreshTokenManager
    {
        public async Task<TokenResponse> RefreshToken(string refreshToken, List<string> extraScopes = null)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            var response = await client.RequestRefreshTokenAsync(BuildRequest(disco, refreshToken, extraScopes));
            return response;
        }

        private RefreshTokenRequest BuildRequest(DiscoveryDocumentResponse disco, string refreshToken, List<string> extraScopes)
        {
            extraScopes.Add("openid");
            extraScopes.Add("profile");
            extraScopes.Add("offline_access");
            if (extraScopes != null && extraScopes.Count() > 0)
            {

                return new RefreshTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    GrantType = "refresh_token",
                    ClientId = "mvc",
                    ClientSecret = "secret",
                    RefreshToken = refreshToken,
                    Scope = string.Join<string>(" ", extraScopes)
                };
            }
            return new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,
                GrantType = "refresh_token",
                ClientId = "mvc",
                ClientSecret = "secret",
                RefreshToken = refreshToken,
            };
        }
    }
}
