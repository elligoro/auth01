using client.logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace client.contracts
{
    public interface IHomeLogic
    {
        Task<AuthTokenResponse> SetAuthCode(string code);
        AuthCodeRedirectModel GetAuthUrl();

        bool ValidateState(Guid state_req, Guid state_res);
    }
}
