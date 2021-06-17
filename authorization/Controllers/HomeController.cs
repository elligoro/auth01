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

namespace authorization.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<AuthTokenResponse> _authToken;
        private readonly AuthContext _context;
        private int _code; // temp. just to check working

        public HomeController(ILogger<HomeController> logger, IOptions<AuthTokenResponse> authToken, AuthContext context)
        {
            _logger = logger;
            _authToken = authToken;
            _context = context;
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
        public async Task<int> GetCode(string client_id)
        {
            var code = new Random().Next(99999, int.MaxValue);
            var dbcodeEntity = await _context.Codes.FindAsync(client_id);
            if (dbcodeEntity is null)
                _context.Codes.Add(new Code
                {
                    code = code,
                    client_id = client_id
                });

            else
                dbcodeEntity.code = code;

            await _context.SaveChangesAsync();

            return code;
        }


        [HttpGet]
        [Route("/token")]
        public async Task<AuthTokenResponse> GetAuthorizationToken()
        {
            //var req = Request;
            //byte[] buffer = new byte[Response.Body.Length];

            string header = Request.Headers["Authorization"];
            header = header.Substring("Basic ".Length).Trim();
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var clientIdSecret = encoding.GetString(Convert.FromBase64String(header));
            var seperator = clientIdSecret.IndexOf(":");
            var clientId = clientIdSecret.Substring(0, seperator);

            using (var reader = new StreamReader(Request.Body))
            {
                var reqBody = reader.ReadToEndAsync().Result;
                reader.Close();

                var codeDbEntity = (await _context.Codes.FindAsync(clientId));

                var reqModel = JsonConvert.DeserializeObject<CodeBodyDto>(reqBody);
                var code = reqModel.code;
                if (code != codeDbEntity.code)
                    return null;

                var resBody = new AuthTokenResponse
                {
                    access_token = _authToken.Value.access_token,
                    token_type = _authToken.Value.token_type
                };

                /*
                var url = JsonConvert.DeserializeObject<CodeBodyDto>(reqBody).redirect_uri;
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                var content = JsonConvert.SerializeObject(resBody);
                httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                */

                return await Task.FromResult(resBody);
            }          
        }
    }

    public class AuthTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; } 
    }

    public class CodeBodyDto
    {
        public string grant_type { get; set; }
        public string redirect_uri { get; set; }
        public int code { get; set; }
    }
}

