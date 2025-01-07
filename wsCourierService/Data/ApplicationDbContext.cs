using Microsoft.EntityFrameworkCore;
using wsCourierService.Models;

namespace wsCourierService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Courier> Couriers { get; set; }
    }
}
