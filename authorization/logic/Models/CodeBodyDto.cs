using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorization.logic.Models
{
    public class CodeBodyDto
    {
        public string grant_type { get; set; }
        public string redirect_uri { get; set; }
        public int code { get; set; }
    }
}
