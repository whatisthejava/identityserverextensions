// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using IdentityServerAspNetIdentity.MyExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServerAspNetIdentity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                .AddExtensionGrantValidator<DelegationGrantValidator>()
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();


            services.AddAuthentication()
           .AddGoogle("Google", options =>
           {
               options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
               options.ClientId = "674890858363-u0p7quog4gr42lksqv27v1fgsd31pumd.apps.googleusercontent.com";
               options.ClientSecret = "-G_Zdh1XyQt8eE5EajH7Hp2l";
           })
           .AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
           {
               options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
               options.SignOutScheme = IdentityServerConstants.SignoutScheme;
               options.SaveTokens = true;

               options.Authority = "https://demo.identityserver.io/";
               options.ClientId = "interactive.confidential";
               options.ClientSecret = "secret";
               options.ResponseType = "code";
           });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}