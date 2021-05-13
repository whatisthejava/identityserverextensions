// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace IdentityServerAspNetIdentity
{
    public class SeedData
    {

       
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlite(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireNonAlphanumeric = false;
            })

                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    bool recreateUsers = true;

                    ManageAlice(userMgr, recreateUsers);
                    ManageBob(userMgr, recreateUsers);
                    ManageCarson(userMgr, recreateUsers);
                }
            }
        }

        private static void ManageCarson(UserManager<ApplicationUser> userMgr, bool delete = false)
        {
            var carson = userMgr.FindByNameAsync("carson").Result;

            if (delete && carson != null)
            {
                _ = userMgr.DeleteAsync(carson).Result;
                carson = null;
            }

            if (carson == null)
            {
                AddCarson(carson, userMgr);
            }
            else
            {
                Log.Debug("carson already exists");
            }
        }


        private static void ManageBob(UserManager<ApplicationUser> userMgr, bool delete = false)
        {
            var bob = userMgr.FindByNameAsync("bob").Result;

            if (delete && bob != null)
            {
                _ = userMgr.DeleteAsync(bob).Result;
                bob = null;
            }

            if (bob == null)
            {
                AddBob(bob, userMgr);
            }
            else
            {
                Log.Debug("bob already exists");
            }
        }

        private static void ManageAlice(UserManager<ApplicationUser> userMgr, bool delete = false)
        {
            var alice = userMgr.FindByNameAsync("alice").Result;

            if (delete && alice != null)
            {
                _ = userMgr.DeleteAsync(alice).Result;
                alice = null;
            }

            if (alice == null)
            {
                AddAlice(alice, userMgr);
            }
            else
            {
                Log.Debug("alice already exists");
            }
        }

        private static void AddAlice(ApplicationUser alice, UserManager<ApplicationUser> userMgr)
        {
            alice = new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(alice, "Sitekit123").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                            new Claim("position", "lock"),
                            new Claim("club", "Exeter"),
                            new Claim("country", "Scotland")
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("alice created");
        }

        private static void AddBob(ApplicationUser bob, UserManager<ApplicationUser> userMgr)
        {
            bob = new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(bob, "Sitekit123").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("position", "prop"),
                            new Claim("club", "Edinburgh"),
                            new Claim("country", "England")
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("bob created");
        }

        private static void AddCarson(ApplicationUser carson, UserManager<ApplicationUser> userMgr)
        {
            carson = new ApplicationUser
            {
                UserName = "carson",
                Email = "Carson@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(carson, "Sitekit123").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(carson, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Carson Wells"),
                            new Claim(JwtClaimTypes.GivenName, "carson"),
                            new Claim(JwtClaimTypes.FamilyName, "Wells"),
                            new Claim(JwtClaimTypes.WebSite, "http://carson.com"),
                            new Claim("position", "Flanker"),
                            new Claim("club", "Leinster"),
                            new Claim("country", "Ireland")
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("carson created");
        }

    }
}
