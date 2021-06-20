using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorization.logic.Models
{
    public class AuthTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
}
