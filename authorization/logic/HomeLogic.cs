using authorization.contracts;
using authorization.DB.Models;
using authorization.logic.Models;
using authorization.persistance;
using contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace logic
{
    public class HomeLogic : IHomeLogic
    {
        private readonly IOptions<AuthTokenResponse> _authToken;
        private readonly AuthContext _context;
        private readonly IHomeDb _homeDb;

        public HomeLogic(
            IOptions<AuthTokenResponse> authToken,
            AuthContext context,
            IHomeDb homeDb
            )
        {
            _authToken = authToken;
            _context = context;
            _homeDb = homeDb;
        }

        public async Task<AuthCodeResponse> GetCode(string client_id, Guid state)
        {
            var code = new Random().Next(99999, int.MaxValue);
            await _homeDb.UpsertCode(client_id, code);

            return new AuthCodeResponse { code = code, 
                                          state = state};
        }

        public async Task<AuthTokenResponse> GetAuthorizationToken(HttpRequest request)
        {
            //var req = Request;
            //byte[] buffer = new byte[Response.Body.Length];

            string header = request.Headers["Authorization"];
            header = header.Substring("Basic ".Length).Trim();
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var clientIdSecret = encoding.GetString(Convert.FromBase64String(header));
            var seperator = clientIdSecret.IndexOf(":");
            var clientId = clientIdSecret.Substring(0, seperator);

            using (var reader = new StreamReader(request.Body))
            {
                var reqBody = reader.ReadToEndAsync().Result;
                reader.Close();

                var codeDbEntity = (await _homeDb.GetCodeDbEntity(clientId));

                var reqModel = JsonConvert.DeserializeObject<CodeBodyDto>(reqBody);
                var code = reqModel.code;
                if (code != codeDbEntity.code)
                    return null;

                var resBody = new AuthTokenResponse
                {
                    access_token = Convert.ToBase64String(Encoding.UTF8.GetBytes("access token" + new Random().Next(999999999, int.MaxValue).ToString())),
                    token_type = _authToken.Value.token_type,
                    refresh_token = Convert.ToBase64String(Encoding.UTF8.GetBytes("refresh token" + new Random().Next(999999999, int.MaxValue).ToString()))
                };

                var tokenDbEntity = new TokenUpdateDbEntity
                {
                    access_token = resBody.access_token,
                    token_type = resBody.token_type,
                    refresh_token = resBody.refresh_token
                };

                await _homeDb.UpdateToken(clientId, tokenDbEntity);

                return await Task.FromResult(resBody);
            }
        }
    }
}
