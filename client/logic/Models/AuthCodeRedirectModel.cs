using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace client.logic.Models
{
    public class AuthCodeRedirectModel
    {
        public AuthorizationDto body { get; set; }
        public string url { get; set; }
    }
}
