using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace client.logic.Models
{
    public class AuthorizationDto
    {
        public string client_id { get; set; }
        public string redirect_uri { get; set; }
        public string response_type { get; set; }
        public Guid state { get; set; }
    }
}
