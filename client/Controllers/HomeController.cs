using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using auth01.Models;
using System.Net.Http;
using auth01.Business;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Policy;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.RazorPages;

using client.logic.Models;
using client.contracts;

namespace auth01.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly HomeHandler _homeHandler;
        private readonly IHomeLogic _homeLogic;
        public HomeController(ILogger<HomeController> logger, IHomeLogic homeLogic)
        {
            _logger = logger;
            _homeLogic = homeLogic;
        }

        public IActionResult Index(AuthTokenResponse authTokenResponse)
        {
            return View(authTokenResponse);
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

        [HttpPost]
        [Route("home/GetAuthUrl")]
        public AuthCodeRedirectModel GetAuthUrl()
        {
            return _homeLogic.GetAuthUrl();
        }

        [HttpGet]
        [Route("/code")]
        public async Task<IActionResult> GetAuthorizationCode()
        {
            Guid state_req_guid;
            Guid state_res_guid;
            Guid.TryParse(Request.Query["state_req"], out state_req_guid);
            Guid.TryParse(Request.Query["state_res"], out state_res_guid);

            if (!_homeLogic.ValidateState(state_req_guid, state_res_guid))
                throw new Exception("state value did not match");
            var dto = await _homeLogic.SetAuthCode(Request.Query["code"]);
            return  RedirectToAction("Index", dto);
        }
    }
}
