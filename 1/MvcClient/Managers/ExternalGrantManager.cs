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
    public class ExternalGrantManager : IExternalGrantManager
    {
        public async Task<string> ExecuteDelegation(HttpContext context)
        {
            var accessToken = await context.GetTokenAsync("access_token");

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://identityserver-sp.azurewebsites.net/");

            var response = await client.RequestTokenAsync(new TokenRequest
            {
                Address = disco.TokenEndpoint,
                GrantType = "delegation",

                ClientId = "api1.client",
                ClientSecret = "secret",

                Parameters =
                {
                    { "scope", "employment" },
                    { "token", accessToken}
                }
            });
            var newToken = response.Json;
            return newToken.GetProperty("access_token").ToString();
        }
    }
}
