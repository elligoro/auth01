using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using authorization.Models;
using System.IO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using authorization.persistance;
using contracts;
using authorization.logic.Models;

namespace authorization.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeLogic _homeLogic;

        public HomeController(ILogger<HomeController> logger, 
                              IHomeLogic homeLogic)
        {
            _logger = logger;
            _homeLogic = homeLogic;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("/authorize")]
        public IActionResult GetAuthorizationCode()
        {
            return View("index");
        }

        [HttpGet]
        [Route("/code/{client_id}")]
        public async Task<AuthCodeResponse> GetCode(string client_id, Guid state)
        {
            return await _homeLogic.GetCode(client_id,state);
        }


        [HttpGet]
        [Route("/token")]
        public async Task<AuthTokenResponse> GetAuthorizationToken()
        {
            return await _homeLogic.GetAuthorizationToken(Request);
        }
    }
}

