using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace authorization.persistance
{
    public class AuthContext: DbContext
    {
       public DbSet<Code> Codes { get; set; }

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }
    }


    public class Code
    {
        [Key]
        public string client_id { get; set; }
        public int? code { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
