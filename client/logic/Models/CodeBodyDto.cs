using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace client.logic.Models
{
    public class CodeBodyDto
    {
        public string grant_type { get; set; }
        public string redirect_uri { get; set; }
        public string code { get; set; }
    }
}
