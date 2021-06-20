using authorization.logic.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace contracts
{
    public interface IHomeLogic
    {
        Task<int> GetCode(string client_id);
        Task<AuthTokenResponse> GetAuthorizationToken(HttpRequest request);
    }
}
