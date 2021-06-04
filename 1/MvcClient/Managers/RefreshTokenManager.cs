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
        public async Task<TokenResponse> RefreshToken(string refreshToken, List<string> scopes = null)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://identityserver-sp.azurewebsites.net/");
            var request = BuildRequest(disco, refreshToken, scopes);
            var response = await client.RequestRefreshTokenAsync(request);
            return response;
        }

        private RefreshTokenRequest BuildRequest(DiscoveryDocumentResponse disco, string refreshToken, List<string> scopes)
        {
            if (scopes != null && scopes.Count() > 0)
            {
                scopes.Add("openid");
                scopes.Add("profile");
                scopes.Add("offline_access");

                return new RefreshTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    GrantType = "refresh_token",
                    ClientId = "mvc",
                    ClientSecret = "secret",
                    RefreshToken = refreshToken,
                    Scope = string.Join<string>(" ", scopes)
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
