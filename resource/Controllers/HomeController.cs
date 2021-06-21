using Microsoft.AspNetCore.Mvc;
using resource.logic.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace resource.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        
        [HttpGet]
        [Route("/")]
        public async Task<ResourceGetResponse> GetResource()
        {
            string header = HttpContext.Request.Headers["Authorization"];
            var token = header.Substring("bearer ".Length).Trim();
            var res = new ResourceGetResponse { TestResult = "result!" };
            if (!(string.IsNullOrEmpty(token)))
                res.TestResult = "have token!";

            return await Task<ResourceGetResponse>.FromResult(res);
        }        
    }
}
