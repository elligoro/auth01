using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace client.logic.Models
{
    public class AuthorizationModel
    {
        public string client_id { get; set; }
        public Dictionary<string, string> redirect_uri { get; set; }
        public string client_secret { get; set; }
        public string AuthorizationEndPoint { get; set; }
        public string TokenEndPoint { get; set; }
    }
}
