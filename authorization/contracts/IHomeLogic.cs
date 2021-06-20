using authorization.logic.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace contracts
{
    public interface IHomeLogic
    {
        Task<AuthCodeResponse> GetCode(string client_id, Guid state);
        Task<AuthTokenResponse> GetAuthorizationToken(HttpRequest request);
    }
}
