using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorization.logic.Models
{
    public class AuthCodeResponse
    {
        public int code { get; set; }
        public Guid state { get; set; }
    }
}
