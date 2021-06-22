using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace resource.persistence
{
    public class ResourceContext : DbContext
    {
        DbSet<Resource> Resources { get; set; }

        public ResourceContext(DbContextOptions<ResourceContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>()
                .HasData(
                    new Resource { Id = 1, Data = "test!" }
                );
        }
    }

    public class Resource
    {
        [Key]
        public int Id { get; set; }
        public string Data { get; set; }
    }
}
