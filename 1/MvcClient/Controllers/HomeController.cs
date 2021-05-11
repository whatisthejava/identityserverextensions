using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public HomeController(ILogger<HomeController> logger, ICallExternalApiManager manager, IExternalGrantManager grants)
        {
            _logger = logger;
            _manager = manager;
            _grants = grants;
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
