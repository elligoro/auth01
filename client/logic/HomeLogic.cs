using client.contracts;
using client.logic.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace logic
{
    public class HomeLogic : IHomeLogic
    {
        private readonly IOptions<AuthorizationModel> _authConfig;

        public HomeLogic(IOptions<AuthorizationModel> authConfig)
        {
            _authConfig = authConfig;
        }

        public async Task<AuthTokenResponse> SetAuthCode(string code)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _authConfig.Value.TokenEndPoint);
            var content = JsonConvert.SerializeObject(new CodeBodyDto
            {
                grant_type = "authorization_code",
                code = code,
                redirect_uri = _authConfig.Value.redirect_uri.GetValueOrDefault("token")
            });
            httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

            var buffer = Encoding.UTF8.GetBytes($"{_authConfig.Value.client_id}:{_authConfig.Value.client_secret }");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(buffer));
            var result = (await new HttpClient().SendAsync(httpRequestMessage)).Content.ReadAsStringAsync();
            if (result is null)
                throw new Exception("problem with authentication proccess!!!");
            return JsonConvert.DeserializeObject<AuthTokenResponse>(result.Result);
        }

        public AuthCodeRedirectModel GetAuthUrl()
        {
            var ac = _authConfig.Value;
            var res = new AuthCodeRedirectModel
            {
                url = ac.AuthorizationEndPoint,
                body = new AuthorizationDto
                {
                    client_id = ac.client_id,
                    response_type = "code",
                    redirect_uri = ac.redirect_uri.GetValueOrDefault("code"),
                    state = Guid.NewGuid()
                }
            };
            return res;
        }

        public bool ValidateState(Guid state_req, Guid state_res) => state_req == state_res;

        public async Task<ProtectedResourceResponse> GetProtectedResource()
        {
            return await Task.FromResult<ProtectedResourceResponse>( new ProtectedResourceResponse { } );
        }
    }
}
