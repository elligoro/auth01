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

namespace auth01.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly HomeHandler _homeHandler;
        private readonly IOptions<AuthorizationModel> _authConfig;
        public HomeController(ILogger<HomeController> logger, 
                              //HomeHandler homeHandler, 
                              IOptions<AuthorizationModel> authConfig)
        {
            _logger = logger;
            //_homeHandler = homeHandler;
            _authConfig = authConfig;
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

        public AuthCodeRedirectModel GetAuthUrl()
        {
            var ac = _authConfig.Value;
            var res = new AuthCodeRedirectModel {
                url = ac.AuthorizationEndPoint,
                body = new AuthorizationDto
                {
                    client_id = ac.client_id,
                    response_type = "code",
                    redirect_uri = ac.redirect_uri.GetValueOrDefault("code")
                }
            };
            return  res;
        }

        [HttpGet]
        [Route("/code")]
        public async Task<IActionResult> GetAuthorizationCode()
        {
            var queryCode = Request.Query["code"];
            var dto = await SetAuthCode(queryCode);
            return  RedirectToAction("Index", dto);
        }

        public async Task<AuthTokenResponse> SetAuthCode(string code)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _authConfig.Value.TokenEndPoint);
            var content = JsonConvert.SerializeObject(new CodeBodyDto { grant_type = "authorization_code", 
                                                                        code = code, 
                                                                        redirect_uri = _authConfig.Value.redirect_uri.GetValueOrDefault("token") });
            httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

            var buffer = Encoding.UTF8.GetBytes($"{_authConfig.Value.client_id}:{_authConfig.Value.client_secret }");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(buffer));
           var result = (await new HttpClient().SendAsync(httpRequestMessage)).Content.ReadAsStringAsync();
            if (result is null)
                throw new Exception("problem with authentication proccess!!!");
            return JsonConvert.DeserializeObject<AuthTokenResponse>(result.Result);
        }
    }

    public class AuthorizationModel
    {
        public string client_id { get; set; }
        public Dictionary<string, string> redirect_uri { get; set; }
        public string client_secret { get; set; }
        public string AuthorizationEndPoint { get; set; }
        public string TokenEndPoint { get; set; }
    }

    public class AuthorizationDto
    {
        public string client_id { get; set; }
        public string redirect_uri { get; set; }
        public string response_type { get; set; }
    }

    public class CodeBodyDto
    {
        public string grant_type { get; set; }
        public string redirect_uri { get; set; }
        public string code { get; set; }
    }

    public class CodeHeaderDto
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
    public class AuthCodeRedirectModel
    {
        public AuthorizationDto body { get; set; }
        public string url { get; set; }
    }

    public class AuthTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
}
