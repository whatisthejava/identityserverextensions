// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.MyExtensions;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                SPIdentityResources.Rugby(),
                SPIdentityResources.Bmi(),
            };



        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API"),
                new ApiScope("employment", "Employment scope for API2")
            };

        public static ICollection<string> PasswordAndCode()
        {
            var l = new List<string>();
            l.AddRange(GrantTypes.Code);
            l.AddRange(GrantTypes.ResourceOwnerPassword);
            return l;
        }
            

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "api1.client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedGrantTypes = { "delegation" },

                    AllowedScopes = new List<string>
                    {
                        "employment"
                    }
                },
                new Client
                {
                    ClientId = "mvc",
                    AllowedGrantTypes = PasswordAndCode(),
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                     RedirectUris = { "https://localhost:5002/signin-oidc" },
                     AllowOfflineAccess = true,
                     PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
                     AlwaysIncludeUserClaimsInIdToken = true,
                     UpdateAccessTokenClaimsOnRefresh = true,
                     AllowedScopes = new List<string>
                     {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "rugby",
                        "bmi",
                        "api1"
                    }
                },
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },
                 new Client
                {
                    ClientId = "client2",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "employment" }
                },
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = PasswordAndCode(),
                    RequireClientSecret = false,

                    RedirectUris =           { "https://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = { "https://localhost:5003/index.html" },
                    AllowedCorsOrigins =     { "https://localhost:5003" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1",
                        "rugby"
                    }
                }
            };
    }
}