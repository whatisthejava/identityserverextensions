using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MvcClient.Managers;
using MvcClient.Models;
using Newtonsoft.Json.Linq;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICallExternalApiManager _manager;
        private readonly IExternalGrantManager _grants;
        private readonly IRefreshTokenManager _refresh;

        public HomeController(ILogger<HomeController> logger, ICallExternalApiManager manager, IExternalGrantManager grants, IRefreshTokenManager refresh)
        {
            _logger = logger;
            _manager = manager;
            _grants = grants;
            _refresh = refresh;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var content = await _manager.CallApi(accessToken, "https://localhost:6001/identity");
            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }

        public async Task<IActionResult> ExtensionGrant()
        {
            var accessToken = await _grants.ExecuteDelegation(HttpContext);
            var content = await _manager.CallApi(accessToken, "https://localhost:6002/identity");
            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }



        [Route("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var tokenResult = await _refresh.RefreshToken(refreshToken);

            var authInfo = await HttpContext.AuthenticateAsync("Cookies");
            authInfo.Properties.UpdateTokenValue(OpenIdConnectParameterNames.AccessToken, tokenResult.AccessToken);
            authInfo.Properties.UpdateTokenValue(OpenIdConnectParameterNames.RefreshToken, tokenResult.RefreshToken);
            await HttpContext.SignInAsync("Cookies", authInfo.Principal, authInfo.Properties);
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
