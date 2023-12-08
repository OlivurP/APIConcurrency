using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorConcurrency.Models;

namespace APIConcurrency.Data
{
    public class APIConcurrencyContext : DbContext
    {
        public APIConcurrencyContext (DbContextOptions<APIConcurrencyContext> options)
            : base(options)
        {
        }

        public DbSet<RazorConcurrency.Models.Movie> Movie { get; set; } = default!;

        public DbSet<RazorConcurrency.Models.Order> Order { get; set; } = default!;
        public DbSet<RazorConcurrency.Models.Seat> Seat { get; set; } = default!;
    }
}
